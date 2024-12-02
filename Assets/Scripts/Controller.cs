using System;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    //Setting up every basic needed variable
    private Rigidbody _rb;
    private Collider _col;
    private GameObject _child;
    private Camera _camera;
    private Vector2 _frameInput;
    private bool _playerInputEnable;
    private bool _isGrounded;

    //Every variable about moving, jumping etc...
    [Header("Move Section")]
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _maxVelocity;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _rushMoveSpeed;
    private float _tempoMoveSpeed;

    //Every variable about camera orientation and limitation
    [Header("Camera Section")]
    [Range(0.1f, 9f)][SerializeField] private float _sensitivity = 1f;
    [Range(0f, 90f)][SerializeField] private float _yRotationLimit = 88f;
    [Range(0.3f, 3f)][SerializeField] private float _distancePickUp = 0.5f;
    private Vector2 _rotation = Vector2.zero;

    //Weapon handling variable
    [Header("Shooting Section")]
    [SerializeField] private Transform _handleWeaponLeft;
    [SerializeField] private Transform _handleWeaponRight;
    [SerializeField] private WeaponBehavior _leftWeapon;
    [SerializeField] private WeaponBehavior _rightWeapon;
    [SerializeField] private WeaponBehavior _toPickUp;

    //Input action section, has to be public or can be private with a SerializeField statement
    [Header("Input Section")]
    public InputAction move;
    public InputAction look;
    public InputAction pause;
    public InputAction jump;
    public InputAction shootLeft;
    public InputAction shootRight;
    public InputAction pickup;
    public InputAction drop;
    public InputAction rush;

    //Not needed to be visible in the Editor
    private Vector2 _lookInput;
    private Vector3 _localMove = Vector3.zero;
    private Vector3 _moveDirection = Vector3.zero;
    private float _sqrMaxVelocity;

    void Awake()
    {
        //Setting every variable in order to work
        _rb = this.GetComponent<Rigidbody>();
        _col = this.GetComponent<Collider>();
        _child = this.transform.GetChild(0).gameObject;
        _camera = this.GetComponentInChildren<Camera>();
        _playerInputEnable = true;
        _tempoMoveSpeed = _moveSpeed;

        //Activating every input
        move.Enable();
        look.Enable();
        pause.Enable();
        jump.Enable();
        shootLeft.Enable();
        shootRight.Enable();
        pickup.Enable();
        drop.Enable();
        rush.Enable();
    }

    void Start()
    {
        //block and hide the cursor in the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        _sqrMaxVelocity = _maxVelocity * _maxVelocity;
    }

    void Update()
    {
        //Pause working, only freezing the inputs. 
        if (pause.WasPressedThisFrame())
            _playerInputEnable = !_playerInputEnable;

        //If inputs are available, handle every inputs inside
        if (_playerInputEnable)
        {
            _frameInput = move.ReadValue<Vector2>();
            weaponInputs(_leftWeapon, shootLeft);
            weaponInputs(_rightWeapon, shootRight);
            if (pickup.WasPressedThisFrame())
                Pickup(_toPickUp);
            if (drop.WasPressedThisFrame())
                Drop();
            if (rush.WasPressedThisFrame())
                StartCoroutine(Rush());
            if (rush.WasReleasedThisFrame())
                _moveSpeed = _tempoMoveSpeed;
            if (jump.WasPressedThisFrame())
                Jump();
            //Look();
        }

        //Methods called on each frame to handle various mechanics 
        IsGrounded();
        CanPickUp();
    }

    private void weaponInputs(WeaponBehavior weapon, InputAction input)
    {
        // Avoid minor code duplication + lets us "disable" a weapon if we ever want
        if (input.WasPressedThisFrame() && weapon != null)
            weapon.Shoot();
        if (input.IsPressed() && !input.WasPressedThisFrame() && weapon != null)
            weapon.ShootHeld();
        if (input.WasReleasedThisFrame() && weapon != null)
            weapon.ReleaseShoot();
    }

    /**
    *   Coroutine Rush to handle movespeed over time and making it feel more realistic
    **/
    private IEnumerator Rush()
    {
        while (_moveSpeed < _rushMoveSpeed)
        {
            _moveSpeed += Time.deltaTime * 2;
            yield return null;
        }
        _moveSpeed = _rushMoveSpeed;
    }

    /**
    *   Only used for physics behavior (whenever it touch a rigidbody, put it here)
    **/
    void FixedUpdate()
    {
        if (_playerInputEnable)
        {
            Move();
            Look();
            if (_rb.linearVelocity.sqrMagnitude > _sqrMaxVelocity)
            {
                _rb.linearVelocity = _rb.linearVelocity.normalized * _maxVelocity;
            }
        }
    }

    /**
    *   Method to handle the pickup, setter of the _weapon variable
    *   @parameter Weapon weapon, the weapon variable used to set our main weapon, if null return
    **/
    private void Pickup(WeaponBehavior weapon)
    {
        if (weapon == null)
            return;

        if (!this._leftWeapon)
        {
            this._leftWeapon = weapon;
            _leftWeapon.PickUp(_handleWeaponLeft);
            return;
        }
        if (!this._rightWeapon)
        {
            this._rightWeapon = weapon;
            _rightWeapon.PickUp(_handleWeaponRight);
            return;
        }
    }

    /**
    *   if main weapon is null, return, else drop it
    **/
    private void Drop()
    {
        if (_leftWeapon != null)
        {
            _leftWeapon.Drop();
        }
        else if (_rightWeapon != null)
        {
            _rightWeapon.Drop();
        }
        _toPickUp = null;
    }

    /**
    *   Simple method to handle the jump
    **/
    private void Jump()
    {
        if (_isGrounded)
            _rb.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.Impulse);
        //_rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _jumpForce, _rb.linearVelocity.z);
    }

    /**
    *   Method that load a raycast from the camera's pivot, along its Z axis and ending at _distancePickUp. 
    *   When a weapon is detected, _toPickUp handle the weapon and it can be picked up with PickUp(weapon)
    **/
    private void CanPickUp()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward * _distancePickUp);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, _distancePickUp))
        {
            if (hit.collider.TryGetComponent<WeaponBehavior>(out WeaponBehavior weapon))
                _toPickUp = weapon;
            else
                _toPickUp = null;
        }
        else
            _toPickUp = null;
    }

    /**
    *   Method to handle the camera orientation along look input.
    **/
    void Look()
    {
        //storing look input value 
        _lookInput = look.ReadValue<Vector2>();

        //giving the look value into another variable used for calculation
        _rotation.x += _lookInput.x * _sensitivity;
        _rotation.y += _lookInput.y * _sensitivity;

        //Clamp, limit the orientation on Y axis to block reversion possibility
        _rotation.y = Mathf.Clamp(_rotation.y, -_yRotationLimit, _yRotationLimit);

        // Get the rotation if the gravity is inverted
        AntiGravityPlayer antiGravityPlayer = GetComponent<AntiGravityPlayer>();
        Quaternion baseRotation = antiGravityPlayer != null ? antiGravityPlayer.RotationGravity : Quaternion.identity;

        //calculating quaternion on every axis
        var xQuat = Quaternion.AngleAxis(_rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(_rotation.y, Vector3.left);

        // If the player is in rotation
        if (antiGravityPlayer.IsRotating)
        {
            //Make the rotation smooth
            transform.localRotation = Quaternion.Lerp(transform.localRotation, baseRotation * xQuat, Time.deltaTime * 5f);

            if (Quaternion.Angle(transform.rotation, baseRotation * xQuat) < 0.1f)
            {
                //finalising orientation through transform
                //Calculate the orientation with gravity orientation
                transform.localRotation = baseRotation * xQuat;
                // The player is no more rotating
                antiGravityPlayer.IsRotating = false; 
            }
        }
        else
        {
            //finalising orientation through transform
            //Calculate the orientation with gravity orientation
            transform.localRotation = Quaternion.Lerp(transform.localRotation, baseRotation * xQuat, Time.deltaTime * 5f);
        }
        //finalising orientation through transform
        _child.transform.localRotation = Quaternion.Lerp(_child.transform.localRotation, yQuat, Time.deltaTime * 5f);
    }

    /**
    *   Simple method to handle the basic movement
    **/
    private void Move()
    {
        _moveDirection = new Vector3(_frameInput.x, 0f, _frameInput.y);
        _localMove = transform.TransformDirection(_moveDirection);
        _rb.AddForce(new Vector3(_localMove.x * _moveSpeed * Time.deltaTime, 0, _localMove.z * _moveSpeed * Time.deltaTime));
        // this._rb.linearVelocity = new Vector3(_localMove.x * _moveSpeed, _rb.linearVelocity.y, _localMove.z * _moveSpeed);
    }

    /**
    *   Raycast from the bounds of this collider, along the Y axis to check if the player is on the ground or no
    **/
    private void IsGrounded()
    {
        _isGrounded = false;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(_col.bounds.center, Vector3.down, out hit, _col.bounds.extents.y + _groundCheckDistance))
            if (hit.collider.CompareTag("Ground"))
                _isGrounded = true;
    }

    /**
    *   Getter of the Rigidbody accessible for the weapons
    **/
    public Rigidbody GetRigidbody()
    {
        return this._rb;
    }
}

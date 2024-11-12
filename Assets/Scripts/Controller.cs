using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _col;
    private GameObject _child;
    private Camera _camera;
    private Vector2 _frameInput;
    private bool _playerInputEnable;
    private bool _isGrounded;
    
    [Header("Move Section")]
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _rushMoveSpeed;
    private float _tempoMoveSpeed;

    [Header("Camera Section")]
    [Range(0.1f, 9f)][SerializeField] private float _sensitivity = 1f;
    [Range(0f, 90f)][SerializeField] private float _yRotationLimit = 88f;
    [Range(0.3f, 3f)][SerializeField] private float _distancePickUp = 0.5f;
    private Vector2 _rotation = Vector2.zero;

    [Header("Shooting Section")]
    [SerializeField] private Transform _handleWeapon;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private Weapon _toPickUp;

    [Header("Input Section")]
    public InputAction move;
    public InputAction look;
    public InputAction pause;
    public InputAction jump;
    public InputAction shoot;
    public InputAction pickup;
    public InputAction drop;
    public InputAction rush;

    private Vector2 _lookInput;
    private Vector3 _localMove = Vector3.zero;
    private Vector3 _moveDirection = Vector3.zero;

    void Awake()
    {
        _rb = this.GetComponent<Rigidbody>();
        _col = this.GetComponent<Collider>();
        _child = this.transform.GetChild(0).gameObject;
        _camera = this.GetComponentInChildren<Camera>();
        _playerInputEnable = true;
        move.Enable();
        look.Enable();
        pause.Enable();
        jump.Enable();
        shoot.Enable();
        pickup.Enable();
        drop.Enable();
        rush.Enable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _tempoMoveSpeed = _moveSpeed;
    }

    void Update()
    {
        if(pause.WasPressedThisFrame())
            _playerInputEnable = !_playerInputEnable;

        if(_playerInputEnable)
        {
            if(jump.WasPressedThisFrame())
                Jump();
            _frameInput = move.ReadValue<Vector2>();
            if(shoot.WasPressedThisFrame() && _weapon != null)
                _weapon.Shoot();
            if(pickup.WasPressedThisFrame())
                Pickup(_toPickUp);
            if(drop.WasPressedThisFrame())
                Drop();
            if(rush.WasPressedThisFrame())
                StartCoroutine(Rush());
            if(rush.WasReleasedThisFrame())
                _moveSpeed = _tempoMoveSpeed;
        }
    }

    private IEnumerator Rush()
    {
        while(_moveSpeed < _rushMoveSpeed)
        {
            _moveSpeed += Time.deltaTime * 2;
            yield return null;
        }
        _moveSpeed = _rushMoveSpeed;
    }

    void FixedUpdate()
    {
        if(_playerInputEnable)
        {
            Move();
            Look();
        }
        IsGrounded();
        CanPickUp();
    }

    private void Pickup(Weapon weapon)
    {
        if(weapon == null)
            return;

        this._weapon = weapon;
        _weapon.PickUp(_handleWeapon);
    }

    private void Drop()
    {
        if(_weapon == null)
            return;
        _toPickUp = null;
        _weapon.Drop();
    }

    private void Jump()
    {
        if(_isGrounded)
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _jumpForce, _rb.linearVelocity.z);
    }

    private void CanPickUp()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward * _distancePickUp);
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(ray, out hit, _distancePickUp))
        {
            if(hit.collider.TryGetComponent<Weapon>(out Weapon weapon))
                _toPickUp = weapon;
            else
                _toPickUp = null;
        }
        else
            _toPickUp = null;
    }

    void Look()
    {
        _lookInput = look.ReadValue<Vector2>();

        _rotation.x += _lookInput.x * _sensitivity;
        _rotation.y += _lookInput.y * _sensitivity;
        _rotation.y = Mathf.Clamp(_rotation.y, -_yRotationLimit, _yRotationLimit);
        var xQuat = Quaternion.AngleAxis(_rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(_rotation.y, Vector3.left);

        transform.localRotation = xQuat;
        _child.transform.localRotation = yQuat;
    }

    private void Move()
    {
        _moveDirection = new Vector3(_frameInput.x, 0f, _frameInput.y);
        _localMove = transform.TransformDirection(_moveDirection);
        this._rb.linearVelocity = new Vector3(_localMove.x * _moveSpeed, _rb.linearVelocity.y, _localMove.z * _moveSpeed);
    }

    private void IsGrounded()
    {
        _isGrounded = false;
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(_col.bounds.center, Vector3.down,out hit, _col.bounds.extents.y + _groundCheckDistance))
            if(hit.collider.CompareTag("Ground"))
                _isGrounded = true;
    }
}

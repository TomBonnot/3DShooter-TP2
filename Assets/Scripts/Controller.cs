using System;
using System.Collections;
using System.Runtime;
using TMPro;
using Tripolygon.UModelerX.Runtime;
using Unity.VisualScripting;
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
    [SerializeField] private float _dodgeSpeed;
    [Range(0f, 1f)][SerializeField] private float _dodgeTime;
    [SerializeField] private float _initialBoostSpeed;
    private float _tempoMoveSpeed;

    //0 References mais je le garde, la mecanique de pickup sera pas la même
    [Range(0.3f, 3f)][SerializeField] private float _distancePickUp = 0.5f;


    //Weapon handling variable
    [Header("Shooting Section")]
    [SerializeField] private Transform _handleWeaponLeft;
    [SerializeField] private Transform _handleWeaponRight;
    [SerializeField] private WeaponBehavior _leftWeapon;
    [SerializeField] private WeaponBehavior _rightWeapon;
    [SerializeField] private GameObject _basicWeaponPrefab;

    [Header("Moving and looking Section")]
    [SerializeField] private GameObject _topBody;
    [SerializeField] private GameObject _lowBody;
    private Vector3 _lastMoveDirection = Vector3.zero;

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
    public InputAction dodgeRoll;
    public InputAction restartLevel;
    public InputAction lookBackwards;
    [Header("Weapon Display Section")]
    public TMP_Text leftWeaponNameText;
    public TMP_Text rightWeaponNameText;
    public TMP_Text leftWeaponAmmoText;
    public TMP_Text rightWeaponAmmoText;

    //Not needed to be visible in the Editor
    private Vector3 _localMove = Vector3.zero;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _dodgeVector = Vector3.zero;
    private float _sqrMaxVelocity;
    private bool _isDodging = false;
    private Targeted targeted;

    private bool _isHoldingBasicLeft;
    private bool _isHoldingBasicRight;
    public bool IsLookingBack { get; private set; }
    private float _currentYRotation;

    void Awake()
    {
        //Setting every variable in order to work
        _rb = this.GetComponent<Rigidbody>();
        _col = this.GetComponent<Collider>();
        _child = this.transform.GetChild(0).gameObject;
        _camera = this.GetComponentInChildren<Camera>();

        _playerInputEnable = true;
        _tempoMoveSpeed = _moveSpeed;
        targeted = new Targeted();
        targeted.target = this.gameObject;
        targeted.targetPoint = Vector3.zero;
        IsLookingBack = false;
        _currentYRotation = 0f;

        // On commence avec deux guns standards
        equipBasicWeapon();
        equipBasicWeapon();

        //Activating every input
        move.Enable();
        look.Enable();
        pause.Enable();
        jump.Enable();
        shootLeft.Enable();
        shootRight.Enable();
        //pickup.Enable();
        drop.Enable();
        rush.Enable();
        dodgeRoll.Enable();
        restartLevel.Enable();
        lookBackwards.Enable();


    }

    private void equipBasicWeapon()
    {
        // Crée un basic weapon et equip le
        WeaponBehavior basicWeapon = Instantiate(_basicWeaponPrefab).GetComponent<WeaponBehavior>();
        Pickup(true, basicWeapon);
    }

    void Start()
    {
        //block and hide the cursor in the middle of the screen
        // Cursor.lockState = CursorLockMode.Locked;
        _sqrMaxVelocity = _maxVelocity * _maxVelocity;

        GameManager.Instance.OnGameOver += EnableDisablePlayerControls;
        GameManager.Instance.OnEnableDisableControllerPlayer += EnableDisablePlayerControls;
    }


    void Update()
    {
        // Get where the player is aiming
        getHovered();
        //Pause working, only freezing the inputs.
        if (pause.WasPressedThisFrame())
        {
            EnableDisablePlayerControls();
            GameManager.Instance.PauseGame();
        }

        manageDisplays();

        //If inputs are available, handle every inputs inside
        if (_playerInputEnable)
        {
            _frameInput = move.ReadValue<Vector2>();
            if (move.WasPressedThisFrame() && _isGrounded)
            {
                _frameInput *= _initialBoostSpeed; // On donne un boost initial de mouvement
            }
            clickInputs(_leftWeapon, shootLeft);
            clickInputs(_rightWeapon, shootRight);
            if (pickup.WasPressedThisFrame())
                Pickup(false);
            if (drop.WasPressedThisFrame())
                Drop();
            if (rush.WasPressedThisFrame())
                StartCoroutine(Rush());
            if (rush.WasReleasedThisFrame())
                _moveSpeed = _tempoMoveSpeed;
            if (jump.WasPressedThisFrame())
                Jump();
            if (dodgeRoll.WasPressedThisFrame() && !_isDodging)
                Dodge();
            if (restartLevel.WasPressedThisFrame())
            {
                EnableDisablePlayerControls();
                GameManager.Instance.RestartLevel();
                Drop();
                Drop();
            }
            HandleLookBackward();
            // if (lookBackwards.WasPressedThisFrame())
            // {
            //     IsLookingBack = !IsLookingBack;
            //     if (IsLookingBack)
            //         transform.rotation = Quaternion.Euler(0, 180f, 0);
            //     else
            //         transform.rotation = Quaternion.Euler(0, 0, 0);
            //     if (GetComponent<AntiGravityPlayer>().IsGravityInverted)
            //     {
            //         transform.rotation = Quaternion.Euler(180f, 0, 0);
            //     }
            // }
            LookAtTarget();
        }

        //Methods called on each frame to handle various mechanics 
        IsGrounded();
    }

    private void HandleLookBackward()
    {
        if (lookBackwards.WasPressedThisFrame())
        {
            IsLookingBack = !IsLookingBack;

            // Calculate the new Y rotation
            _currentYRotation = IsLookingBack ? 180f : 0f;

            // Apply rotation based on gravity state
            if (GetComponent<AntiGravityPlayer>().IsGravityInverted)
            {
                transform.rotation = Quaternion.Euler(180f, _currentYRotation, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, _currentYRotation, 0f);
            }
        }
    }
    public void SyncRotationWithGravity()
    {
        if (GetComponent<AntiGravityPlayer>().IsGravityInverted)
        {
            transform.rotation = Quaternion.Euler(180f, _currentYRotation, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, _currentYRotation, 0f);
        }
    }


    private void clickInputs(WeaponBehavior weapon, InputAction input)
    {
        // Avoid minor code duplication + lets us "disable" a weapon if we ever want
        if (input.WasPressedThisFrame() && weapon != null)
            weapon.Shoot();
        if (input.IsPressed() && !input.WasPressedThisFrame() && weapon != null)
            weapon.ShootHeld();
        if (input.WasReleasedThisFrame() && weapon != null)
            weapon.ReleaseShoot();
    }

    private void manageDisplays()
    {
        if (this._leftWeapon)
        {
            this.leftWeaponNameText.text = this._leftWeapon.representName();
            this.leftWeaponAmmoText.text = this._leftWeapon.representAmmo();
        }
        if (this._rightWeapon)
        {
            this.rightWeaponNameText.text = this._rightWeapon.representName();
            this.rightWeaponAmmoText.text = this._rightWeapon.representAmmo();
        }

    }

    private void getHovered()
    {
        // Stores the game object that is being targeted as well as the position of the pointed point (?) into Targeted.
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit playerHit, Mathf.Infinity, LayerMask.GetMask("Player")))
        {
            int _gravityValue = GetComponent<AntiGravityPlayer>().IsGravityInverted ? 10 : -10;
            targeted.targetPoint = new Vector3(transform.position.x, transform.position.y + _gravityValue, transform.position.z);
        }
        // Perform a raycast
        else if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~LayerMask.GetMask("Player", "Ignore Raycast")))
        {
            targeted.target = hit.collider.gameObject;
            targeted.targetPoint = hit.point;
        }
    }

    private void LookAtTarget()
    {
        // Vérifiez si la cible est un enfant de TopBody
        if (targeted.target && targeted.target.transform.IsChildOf(_topBody.transform))
        {
            // Redirigez le regard vers un point par défaut ou continuez à regarder la position précédente
            return;
        }

        // Appliquez le LookAt normalement si la cible est valide
        _topBody.transform.LookAt(this.getTargeted().targetPoint);
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
        if (_playerInputEnable && !_isDodging)
        {
            Move();
            if (_rb.linearVelocity.sqrMagnitude > _sqrMaxVelocity && _isGrounded)
            {
                _rb.linearVelocity = _rb.linearVelocity.normalized * _maxVelocity;
            }
        }
    }

    private void Dodge()
    {
        setLocalMove();
        if (!_isGrounded || (_localMove.x == 0 && _localMove.z == 0)) return; // Si on est pas par terre ou on bouge pas on dodge pas
        _isDodging = true;
        _dodgeVector = new Vector3(_localMove.x, 0, _localMove.z) * Time.deltaTime * _dodgeSpeed * _moveSpeed;
        _rb.AddForce(_dodgeVector);
        StartCoroutine(stopDodging(_dodgeTime));
    }

    private IEnumerator stopDodging(float s)
    {
        //float savedDodgeRotation = dodgeRotationMax;
        yield return new WaitForSeconds(s / 2);
        //dodgeRotationMax = 0;
        yield return new WaitForSeconds(s / 2);
        _isDodging = false;
        //dodgeRotationMax = savedDodgeRotation;
    }

    /**
    *   Method to handle the pickup, setter of the _weapon variable
    *   @parameter Weapon weapon, the weapon variable used to set our main weapon.
        @parameter isBasic, wether or not the weapon being equipped is a "basic" one.
    **/
    private void Pickup(bool isBasic, WeaponBehavior weaponBehavior = null)
    {
        // if we are not looking at a weapon AND we are not forcing a weapon equip
        // if (!targeted.target.CompareTag(Tags.WEAPON) && weaponBehavior == null) return;

        // Si on regarde pas un weapon ou on en set pas un explicitement
        // if (weaponBehavior == null)
        //     weaponBehavior = targeted.target.GetComponent<WeaponBehavior>();

        // This part of the method is to change if we let the player choose which arm to equip their weapon.
        // Has no impact on gameplay, alors jme suis pas cassé la tête.
        if (!this._leftWeapon || !this._leftWeapon.isActiveAndEnabled || (_isHoldingBasicLeft && !isBasic)) // Si tu as RIEN de equip (même pas un basicWeapon) OU tu tiens un basic gun et ne compte pas le remplacer par un autre basic gun
        {
            this._leftWeapon?.Drop();
            _isHoldingBasicLeft = isBasic;
            this._leftWeapon = weaponBehavior;
            this._leftWeapon.PickUp(_handleWeaponLeft);
        }
        else if (!this._rightWeapon || !this._rightWeapon.isActiveAndEnabled || (_isHoldingBasicRight && !isBasic))
        {
            this._rightWeapon?.Drop();
            _isHoldingBasicRight = isBasic;
            this._rightWeapon = weaponBehavior;
            this._rightWeapon.PickUp(_handleWeaponRight);
        }
    }

    /**
    *   if main weapon is null, return, else drop it
    **/
    public void Drop()
    {
        // Pourquoi au lieu de ça je peux pas juste drop ensuite call la méthode Pickup avec un nouveau basic weapon?
        // Je sais pas, alors on a un peu de code duplication
        if (!_isHoldingBasicLeft)
        {
            WeaponBehavior basicWeapon = Instantiate(_basicWeaponPrefab).GetComponent<WeaponBehavior>();
            this._leftWeapon?.Drop();
            _isHoldingBasicLeft = true;
            this._leftWeapon = basicWeapon;
            this._leftWeapon.PickUp(_handleWeaponLeft);
        }
        else if (!_isHoldingBasicRight)
        {
            WeaponBehavior basicWeapon = Instantiate(_basicWeaponPrefab).GetComponent<WeaponBehavior>();
            this._rightWeapon?.Drop();
            _isHoldingBasicRight = true;
            this._rightWeapon = basicWeapon;
            this._rightWeapon.PickUp(_handleWeaponRight);
        }
    }

    public void dropDepletedWeapon(WeaponBehavior weap)
    {
        weap.Drop();
        equipBasicWeapon();
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
    *   Simple method to handle the basic movement
    **/
    private void Move()
    {
        setLocalMove();
        if (_moveDirection.x == 0 && _moveDirection.z == 0)
        {
            _col.material.staticFriction = 0.9f;
            _col.material.dynamicFriction = 0.9f;
            _col.material.frictionCombine = PhysicsMaterialCombine.Maximum;
        }
        else
        {
            _col.material.staticFriction = 0f;
            _col.material.dynamicFriction = 0f;
            _col.material.frictionCombine = PhysicsMaterialCombine.Average;
        }
        if (_moveDirection != Vector3.zero)
        {
            _lastMoveDirection = _moveDirection;
        }
        if (_lastMoveDirection != Vector3.zero)
        {
            Quaternion _targetRotation = Quaternion.LookRotation(_lastMoveDirection);
            _lowBody.transform.localRotation = Quaternion.Slerp(_lowBody.transform.localRotation, _targetRotation, 10 * Time.deltaTime);
        }

        _rb.AddForce(new Vector3(_localMove.x * _moveSpeed * Time.deltaTime, 0, _localMove.z * _moveSpeed * Time.deltaTime));
    }

    public void ResetLastMoveDirection()
    {
        _lastMoveDirection = Vector3.zero;
    }

    private void setLocalMove()
    {
        _moveDirection = new Vector3(_frameInput.x, 0f, _frameInput.y);
        _localMove = transform.TransformDirection(_moveDirection);
    }

    /**
    *   Raycast from the bounds of this collider, along the Y axis to check if the player is on the ground or no
    **/
    private void IsGrounded()
    {
        _isGrounded = false;
        RaycastHit hit = new RaycastHit();
        Vector3 checkDirection = GetComponent<AntiGravityPlayer>().GetGroundCheckDirection();
        if (Physics.Raycast(_col.bounds.center, checkDirection, out hit, _col.bounds.extents.y + _groundCheckDistance))
            if (hit.collider.CompareTag("Ground"))
                _isGrounded = true;
        //_isGrounded = false;
        //RaycastHit hit = new RaycastHit();
        //if (Physics.Raycast(_col.bounds.center, Vector3.down, out hit, _col.bounds.extents.y + _groundCheckDistance))
        //    if (hit.collider.CompareTag("Ground"))
        //        _isGrounded = true;
    }

    /**
    *   Getter of the Rigidbody accessible for the weapons
    **/
    public Rigidbody GetRigidbody()
    {
        return this._rb;
    }

    public void EnableDisablePlayerControls()
    {
        _playerInputEnable = !_playerInputEnable;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(Tags.WEAPON_SPAWNER))
        {
            WeaponBehavior weap = col.gameObject.GetComponentInChildren<WeaponBehavior>();
            if (weap)
                Pickup(false, weap);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag(Tags.WEAPON_SPAWNER))
        {
            col.gameObject.GetComponent<WeaponSpawnerBehavior>().resetWeapon();
        }
    }
    private void OnDisable()
    {
        GameManager.Instance.OnGameOver -= EnableDisablePlayerControls;
        GameManager.Instance.OnEnableDisableControllerPlayer -= EnableDisablePlayerControls;
    }

    /*
    *   Useful to check during collisions. Lets us deal with I-frames if we ever want
    */
    public bool GetIsInvulnerable()
    {
        return _isDodging;
    }

    public Targeted getTargeted()
    {
        return targeted;
    }

    public bool getIsInvertedGravity()
    {
        return GetComponent<AntiGravityPlayer>().IsGravityInverted;
    }
}

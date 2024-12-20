using UnityEngine;

public class AntiGravityPlayer : MonoBehaviour
{
    private Rigidbody _rb;
    private Controller _controller;
    private bool _isGravityInverted;
    private float _gravityForce = 9.8f;
    private float _rotationSpeed = 8f; // Adjust to control rotation speed

    public Quaternion TargetRotation { get; private set; }
    public bool IsRotating { get; private set; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _controller = GetComponent<Controller>();
        _isGravityInverted = false;
        TargetRotation = Quaternion.identity;
        GameManager.Instance.OnReloadLevel += ResetNormalGravity;
    }

    private void FixedUpdate()
    {
        if (_isGravityInverted)
        {
            // Apply inverse gravity
            _rb.AddForce(Vector3.up * _gravityForce * 2f, ForceMode.Acceleration);
        }

        // Handle rotation
        if (IsRotating)
        {
            // Smoothly rotate to target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation, Time.fixedDeltaTime * _rotationSpeed);

            // Check if we're close enough to target rotation
            if (Quaternion.Angle(transform.rotation, TargetRotation) < 1f)
            {
                transform.rotation = TargetRotation;
                IsRotating = false;
                // Re-enable player controls once rotation is complete
                _controller.EnableDisablePlayerControls();
            }
        }
    }

    private void ResetNormalGravity()
    {
        if (_isGravityInverted)
        {
            _isGravityInverted = false;
        }
    }

    public void ChangeGravity()
    {
        _isGravityInverted = !_isGravityInverted;

        // Calculate new target rotation
        if (_isGravityInverted)
        {
            TargetRotation = Quaternion.Euler(180f, transform.rotation.eulerAngles.y, 0f);
        }
        else
        {
            TargetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }

        // Start rotation and temporarily disable player controls
        IsRotating = true;
        _controller.EnableDisablePlayerControls();

        // Reset the last move direction to prevent unwanted rotation
        _controller.ResetLastMoveDirection();
    }
    public Vector3 GetGroundCheckDirection()
    {
        return _isGravityInverted ? Vector3.up : Vector3.down;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnReloadLevel -= ResetNormalGravity;
    }
}
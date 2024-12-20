using UnityEngine;

public class AntiGravityPlayer : MonoBehaviour
{
    private Rigidbody _rb;
    private Controller _controller;
    public bool IsGravityInverted { get; private set; }
    private float _gravityForce = 9.8f;
    private float _rotationSpeed = 8f; // Adjust to control rotation speed

    public Quaternion TargetRotation { get; private set; }
    public bool IsRotating { get; private set; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _controller = GetComponent<Controller>();
        IsGravityInverted = false;
        TargetRotation = Quaternion.identity;
        GameManager.Instance.OnReloadLevel += ResetNormalGravity;
    }

    private void FixedUpdate()
    {
        if (IsGravityInverted)
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
        if (IsGravityInverted)
        {
            IsGravityInverted = false;
        }
    }

    public void ChangeGravity()
    {
        IsGravityInverted = !IsGravityInverted;

        // Calculate new target rotation
        float fixedYRotation = IsGravityInverted ? 180f : 0f;
        TargetRotation = Quaternion.Euler(IsGravityInverted ? 180f : 0f, fixedYRotation, 0f);
        //if (_isGravityInverted)
        //{
        //    TargetRotation = Quaternion.Euler(180f, transform.rotation.eulerAngles.y, 0f);
        //}
        //else
        //{
        //    TargetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        //}

        // Start rotation and temporarily disable player controls
        IsRotating = true;
        _controller.EnableDisablePlayerControls();

        // Reset the last move direction to prevent unwanted rotation
        _controller.ResetLastMoveDirection();
    }
    public Vector3 GetGroundCheckDirection()
    {
        return IsGravityInverted ? Vector3.up : Vector3.down;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnReloadLevel -= ResetNormalGravity;
    }
}
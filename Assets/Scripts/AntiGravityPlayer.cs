using UnityEngine;

public class AntiGravityPlayer : MonoBehaviour
{
    // Manages the player's physics and behavior when gravity is inverted.

    private Rigidbody _rb;
    private bool _isGravityInverted;
    private float _gravityForce;

    // Represents the rotation to apply to the player when gravity is inverted or restored.
    public Quaternion RotationGravity { get; private set; }

    // Indicates whether the player is currently in the process of rotating.
    public bool IsRotating {  get; set; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _isGravityInverted = false;
        _gravityForce = 9.8f;
        RotationGravity = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        if (_isGravityInverted)
        {
            // Apply an upward force to counteract the default gravity
            _rb.AddForce(Vector3.up * _gravityForce * 2, ForceMode.Acceleration);
        }
    }

    // Toggles the direction of gravity for the player. 
    // Updates the player's rotation to ensure they are oriented correctly based on the current gravity direction.
    public void ChangeGravity()
    {
        _isGravityInverted = !_isGravityInverted;

        if (_isGravityInverted)
        {
            RotationGravity = Quaternion.Euler(180f, 0f, 0f);
        }
        else
        {
            RotationGravity = Quaternion.identity;
        }
        IsRotating = true;
    }
}
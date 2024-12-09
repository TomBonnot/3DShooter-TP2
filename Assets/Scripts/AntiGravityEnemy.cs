using UnityEngine;

public class AntiGravityEnemy : MonoBehaviour
{
    // Manages the enemy's physics and behavior when gravity is inverted.

    private Rigidbody _rb;
    public bool IsGravityInverted { get; private set; }
    private float _gravityForce;

    // Represents the rotation to apply to the enemy when gravity is inverted or restored.
    private Quaternion _rotationGravity;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        IsGravityInverted = false;
        _gravityForce = 9.8f;
        _rotationGravity = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        if (IsGravityInverted)
        {
            // Apply an upward force to counteract the default gravity
            _rb.AddForce(Vector3.up * _gravityForce * 2, ForceMode.Acceleration);
        }
        transform.rotation = _rotationGravity;
    }

    // Toggles the direction of gravity for the enemy. 
    // Updates the enemy's rotation to ensure they are oriented correctly based on the current gravity direction.
    public void ChangeGravity()
    {
        IsGravityInverted = !IsGravityInverted;

        if (IsGravityInverted)
        {
            _rotationGravity = Quaternion.Euler(180f, 0f, 0f);
        }
        else
        {
            _rotationGravity = Quaternion.identity;
        }
    }
}

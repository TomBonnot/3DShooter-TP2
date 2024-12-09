using UnityEngine;

public class AntiGravityObject : MonoBehaviour
{
    // Controls the physics of objects whose gravity can be inverted.

    private Rigidbody _rb;    
    private bool _isGravityInverted;
    private float _gravityForce;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _isGravityInverted = false;
        _gravityForce = 9.8f;
    }

    private void FixedUpdate()
    {
        if (_isGravityInverted)
        {
            // Apply an upward force to counteract the default gravity
            _rb.AddForce(Vector3.up * _gravityForce * 2, ForceMode.Acceleration);
        }
    }

    // Toggles the state of gravity inversion for the object.
    public void ChangeGravity()
    {
        _isGravityInverted = !_isGravityInverted;
    }
}

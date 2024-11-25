using UnityEngine;

public class AntiGravityObject : MonoBehaviour
{
    // G�re la physique des objets dont la gravit� peut �tre chang� dans Unity

    private Rigidbody _rb;
    private bool _isGravityInverted;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _isGravityInverted = false;
    }

    private void FixedUpdate()
    {
        if (_isGravityInverted)
        {
            _rb.AddForce(Vector3.up * 9.8f * 2, ForceMode.Acceleration);
        }
    }
    public void ChangeGravity()
    {
        _isGravityInverted = !_isGravityInverted;
    }
}

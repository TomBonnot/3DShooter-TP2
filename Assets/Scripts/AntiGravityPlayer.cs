using UnityEngine;

public class AntiGravityPlayer : MonoBehaviour
{
    // Gère la physique du joueur lorsque sa gravité est changé dans Unity

    private Rigidbody _rb;
    
    //private bool _isRotating;
    private Quaternion _rotation;
    private float _flipSpeed;
    private bool _isGravityInverted;

    Controller _controller;
    public Quaternion BaseRotation { get; private set; }
    public  bool _isRotating {  get; set; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _isGravityInverted = false;
        _flipSpeed = 5f;
        BaseRotation = Quaternion.identity;
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

        if (_isGravityInverted)
        {
            BaseRotation = Quaternion.Euler(180f, 0f, 0f);
        }
        else
        {
            BaseRotation = Quaternion.identity;
        }
        _isRotating = true;
    }
}
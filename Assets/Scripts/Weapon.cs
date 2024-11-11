using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Rigidbody _bulletPrefab;
    [SerializeField] private GameObject _gunPoint;
    [Range(50, 300)] [SerializeField] private float _bulletSpeed = 70f;
    private Rigidbody _bullet;
    private Rigidbody _rb;
    private RigidbodyConstraints _originalConstraints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = this.gameObject.GetComponent<Rigidbody>();
        _originalConstraints = _rb.constraints;
    }

    public void Shoot()
    {
        _bullet = Instantiate(_bulletPrefab, _gunPoint.transform.position, _gunPoint.transform.rotation);
        _bullet.linearVelocity = _gunPoint.transform.up * _bulletSpeed;
    }

    public void PickUp(Transform localParent)
    {
        transform.SetParent(localParent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        _rb.linearVelocity = Vector3.zero;
        _rb.freezeRotation = true;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _rb.useGravity = false;
    }
    
    public void Drop()
    {
        _rb.useGravity = true;
        _rb.constraints = _originalConstraints;
        transform.SetParent(null);
    }
}

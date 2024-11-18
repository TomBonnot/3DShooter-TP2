using UnityEngine;
using UnityEngine.Animations;

public abstract class WeaponBehavior : MonoBehaviour
{
    //Main variable visible on editor
    [SerializeField] protected GameObject _gunPoint;
    [SerializeField] protected GameObject _weapon;
    [Range(50, 300)][SerializeField] protected float _bulletSpeed = 70f;

    //Every physics variable used
    protected Rigidbody _rb;
    protected RigidbodyConstraints _originalConstraints;
    public Projectile projectile;

    //Setting up the variable
    void Start()
    {
        _rb = this.gameObject.GetComponent<Rigidbody>();
        _originalConstraints = _rb.constraints;
    }

    /**
    *   Simple method pubicly accessible (for Controller mainly) to shoot with the weapon
    **/
    public virtual void Shoot()
    {
        GameObject shotProjectile = Instantiate(projectile._projectilePrefab, projectile._projectilePrefab.transform.position, _weapon.transform.rotation);
        shotProjectile.GetComponent<Rigidbody>().linearVelocity = _gunPoint.transform.up * _bulletSpeed;
    }

    /**
    *   Simple method pubicly accessible (for Controller mainly) to pick up the weapon
    **/
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

    /**
    *   Simple method pubicly accessible (for Controller mainly) to drop the weapon
    **/
    public void Drop()
    {
        _rb.useGravity = true;
        _rb.constraints = _originalConstraints;
        transform.SetParent(null);
    }
}

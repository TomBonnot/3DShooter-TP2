using System;
using UnityEngine;
using UnityEngine.Animations;

public class WeaponBehavior : MonoBehaviour
{
    /** 
    Ici on gère la logique Unity d'un Weapon en général
    Ex: Chaque weapon a un rigidbody qui est physicalisé
    Ne devrait jamais être directement associé à un objet dans la scène.
    Toujours utiliser child classes
    **/

    //Main variable visible on editor
    [SerializeField] protected GameObject _gunPoint;
    [SerializeField] protected GameObject _weaponPrefab;
    [SerializeField] protected GameObject _projectile;
    [Range(50, 300)][SerializeField] protected float _bulletSpeed = 70f;
    [Range(1, 10)][SerializeField] protected float _bulletLieftime = 3f;
    [SerializeField] protected int _maxAmmo;


    //Every physics variable used
    protected Rigidbody _rb;
    protected RigidbodyConstraints _originalConstraints;

    // Inner logic classes
    public Projectile projectile;
    public Weapon weapon;

    protected void Start()
    {
        _rb = this.gameObject.GetComponent<Rigidbody>();
        _originalConstraints = _rb.constraints;
    }

    /**
    *   Simple method pubicly accessible (for Controller mainly) to shoot with the weapon
    **/
    public virtual void Shoot()
    {
        throw new NotImplementedException();
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

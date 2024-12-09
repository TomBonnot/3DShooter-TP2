using UnityEngine;
using UnityEngine.Animations;

public class BasicGunBehavior : WeaponBehavior
{
    /** 
    Ici on gère la logique Unity d'un BasicGun
    **/

    void Awake()
    {
        this.weapon = new BasicGun(_weaponPrefab, _gunPoint, _maxAmmo);
    }

    public override void Shoot()
    {
        GameObject shotProjectile = Instantiate(_projectilePrefab, weapon.gunPoint.transform.position, weapon.weaponObject.transform.rotation);
        shotProjectile.GetComponent<Rigidbody>().linearVelocity = _gunPoint.transform.up * _bulletSpeed;
        BasicProjectile projectile = new BasicProjectile(_bulletSpeed);
        Destroy(shotProjectile, _bulletLieftime);
    }
}

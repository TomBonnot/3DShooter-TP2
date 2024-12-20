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
        _soundEmitter.Play();
        // Get the direction from gun point to target
        Vector3 shootDirection = (_playerController.getTargeted().targetPoint - weapon.gunPoint.transform.position).normalized;

        GameObject shotProjectile = Instantiate(_projectilePrefab, weapon.gunPoint.transform.position, Quaternion.LookRotation(shootDirection));
        shotProjectile.GetComponent<Rigidbody>().linearVelocity = shootDirection * _bulletSpeed;
        BasicProjectile projectile = new BasicProjectile(_bulletSpeed);
        Destroy(shotProjectile, _bulletLieftime);
    }

    public override string representAmmo()
    {
        return "Math.Inf";
    }

    public override string representName()
    {
        return "Nail Gun";
    }
}

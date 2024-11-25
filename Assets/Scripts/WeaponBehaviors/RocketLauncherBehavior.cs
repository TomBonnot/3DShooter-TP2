using UnityEngine;
using UnityEngine.Animations;

public class RocketLauncherBehavior : WeaponBehavior
{
    /** 
    Ici on gère la logique Unity d'un Rocket
    Ex: Quand tu shoot on instantie un rocket
    **/

    //Main variable visible on editor
    [Range(50, 300)][SerializeField] protected float explosionStrength = 50f;

    void Awake()
    {
        // this.projectile = new Rocket(_projectilePrefab, _bulletSpeed, _bulletLieftime, explosionStrength);
        this.weapon = new RocketLauncher(_weaponPrefab, _gunPoint, projectile, _maxAmmo);
    }

    public override void Shoot()
    {
        GameObject shotProjectile = Instantiate(_projectilePrefab, weapon.gunPoint.transform.position, weapon.weaponObject.transform.rotation);
        shotProjectile.GetComponent<Rigidbody>().linearVelocity = _gunPoint.transform.up * _bulletSpeed;
        Rocket rocket = new Rocket(_bulletSpeed, _bulletLieftime, explosionStrength);
        Destroy(shotProjectile, _bulletLieftime);
    }
}

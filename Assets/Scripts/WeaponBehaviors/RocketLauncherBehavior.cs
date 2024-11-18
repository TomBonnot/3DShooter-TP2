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
        this.projectile = new Rocket(_projectile, _bulletSpeed, _bulletLieftime, explosionStrength);
        this.weapon = new RocketLauncher(_weaponPrefab, _gunPoint, projectile, _maxAmmo);
    }

    public override void Shoot()
    {
        GameObject shotProjectile = Instantiate(projectile._projectilePrefab, weapon.gunPoint.transform.position, weapon.weaponObject.transform.rotation);
        shotProjectile.GetComponent<Rigidbody>().linearVelocity = _gunPoint.transform.up * _bulletSpeed;
        Destroy(shotProjectile, _bulletLieftime);
    }
}

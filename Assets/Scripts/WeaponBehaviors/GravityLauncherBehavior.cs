using UnityEngine;

public class GravityLauncherBehavior : WeaponBehavior
{
    /** 
    This class manage the behavior of the gravity gun
    **/

    //Main variable visible on editor
    [Range(0, 20)][SerializeField] protected float _radiusExplosion = 5f;

    void Awake()
    {        
        // Assign the right weapon
        this.weapon = new GravityLauncher(_weaponPrefab, _gunPoint, _maxAmmo);
    }

    // Manage the shooting behavior of the gun
    public override void Shoot()
    {
        GameObject shotProjectile = Instantiate(_projectilePrefab, weapon.gunPoint.transform.position, weapon.weaponObject.transform.rotation);
        shotProjectile.GetComponent<Rigidbody>().linearVelocity = _gunPoint.transform.up * _bulletSpeed;

        GravityProjectile gravityProjectile = new GravityProjectile(_bulletSpeed, _bulletLieftime, _radiusExplosion);
        shotProjectile.GetComponent<GravityBehavior>().SetProjectile(gravityProjectile);

        Destroy(shotProjectile, _bulletLieftime);
    }
}

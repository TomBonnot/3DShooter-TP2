using UnityEngine;

public class GravityLauncherBehavior : WeaponBehavior
{
    /** 
    This class manage the behavior of the gravity gun
    **/

    //Main variable visible on editor
    [Range(0, 20)][SerializeField] protected float _radiusExplosion = 3f;
    [SerializeField] protected GameObject _explosion_prefab;

    void Awake()
    {        
        // Assign the right weapon
        this.weapon = new GravityLauncher(_weaponPrefab, _gunPoint, _maxAmmo);
    }

    // Manage the shooting behavior of the gun
    public override void Shoot()
    {
        _soundEmitter.Play();
        GameObject shotProjectile = Instantiate(_projectilePrefab, weapon.gunPoint.transform.position, weapon.weaponObject.transform.rotation);
        shotProjectile.GetComponent<Rigidbody>().linearVelocity = _gunPoint.transform.up * _bulletSpeed;

        GravityProjectile gravityProjectile = new GravityProjectile(_bulletSpeed, _bulletLieftime, _radiusExplosion, _explosion_prefab);
        shotProjectile.GetComponent<GravityProjectileBehavior>().SetProjectile(gravityProjectile);

        Destroy(shotProjectile, _bulletLieftime);
    }
}

using UnityEngine;

public class GrenadeLauncherBehavior : WeaponBehavior
{
    /** 
    This class manage the behavior of the grenade launcher.
    **/

    //Main variable visible on editor
    [Range(0, 20)][SerializeField] protected float _radiusExplosion = 3f;
    [Range(10, 1000)][SerializeField] protected float _explosionForce;
    [SerializeField] protected GameObject _explosion_prefab;

    // The grenade that's been launched
    private GameObject _activeGrenade;

    void Awake()
    {
        // Assign the right weapon
        this.weapon = new GrenadeLauncher(_weaponPrefab, _gunPoint, _maxAmmo);
    }

    // Manage the shooting behavior of the grnade launcher
    public override void Shoot()
    {
        if (_activeGrenade == null)
        {
            _activeGrenade = Instantiate(_projectilePrefab, weapon.gunPoint.transform.position, weapon.weaponObject.transform.rotation);
            _activeGrenade.GetComponent<Rigidbody>().linearVelocity = _gunPoint.transform.up * _bulletSpeed;

            Grenade _grenade = new Grenade(_bulletSpeed, _bulletLieftime, _radiusExplosion, _explosionForce, _explosion_prefab);
            _activeGrenade.GetComponent<GrenadeBehavior>().SetProjectile(_grenade);
        }
        else
        {
            _activeGrenade.GetComponent<GrenadeBehavior>().Explode();
            _activeGrenade = null;
        }
        //Destroy(_activeGrenade, _bulletLieftime);
    }
}

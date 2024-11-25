using UnityEngine;

public class GravityLauncherBehavior : WeaponBehavior
{
    /** 
    Ici on g�re la logique Unity d'un inverseur de gravit�
    Ex: Quand tu shoot on instantie un inverseur de gravit�
    **/

    //Main variable visible on editor
    [Range(0, 20)][SerializeField] protected float _radiusExplosion = 5f;

    void Awake()
    {
        this.projectile = new Gravity(_projectile, _bulletSpeed, _bulletLieftime, _radiusExplosion);
        this.weapon = new GravityLauncher(_weaponPrefab, _gunPoint, projectile, _maxAmmo);
    }

    public override void Shoot()
    {
        GameObject shotProjectile = Instantiate(projectile._projectilePrefab, weapon.gunPoint.transform.position, weapon.weaponObject.transform.rotation);
        shotProjectile.GetComponent<Rigidbody>().linearVelocity = _gunPoint.transform.up * _bulletSpeed;
        Destroy(shotProjectile, _bulletLieftime);
    }
}

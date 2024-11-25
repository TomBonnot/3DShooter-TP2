using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Diagnostics;

public class FlamethrowerBehavior : WeaponBehavior
{
    /** 
    Ici on gère la logique Unity d'un Flamethrower
    **/

    //Main variable visible on editor
    [Range(0, 300)][SerializeField] protected float boostForceHorizontal = 5f;
    [Range(0, 300)][SerializeField] protected float boostForceVertical = 5f;
    private Rigidbody playerRB;
    private GameObject flame;
    private Flamethrower flamethrower;

    void Awake()
    {
        this.projectile = new Flame(_projectile, _bulletSpeed, _bulletLieftime);
        this.flamethrower = new Flamethrower(_weaponPrefab, _gunPoint, projectile, _maxAmmo, boostForceVertical, boostForceHorizontal);
        this.weapon = this.flamethrower;
        playerRB = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
    }

    public override void Shoot()
    {
        flame = Instantiate(projectile._projectilePrefab, weapon.gunPoint.transform);
    }

    public override void ShootHeld()
    {
        // Hover boost
        if (!weapon.expendsAmmo()) { destroyFlame(); return; }
        Vector3 horizVector = boostHorizontally();
        Vector3 vertVector = boostVertically();
        playerRB.AddForce(horizVector + vertVector, ForceMode.Force);
    }

    public override void ReleaseShoot()
    {
        destroyFlame();
    }

    private void destroyFlame()
    {
        if (flame)
            Destroy(flame);
    }

    private Vector3 boostVertically()
    {
        return new Vector3(0, flamethrower.getVerticalBoostStrength(playerRB.linearVelocity), 0);
    }

    private Vector3 boostHorizontally()
    {
        return new Vector3(weapon.gunPoint.transform.up.x, 0, weapon.gunPoint.transform.up.z) * -1 * flamethrower.getHotizontalBoostStrength();
    }
}

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

    private GameObject player;
    private Rigidbody playerRB;

    private GameObject flame;

    void Awake()
    {
        this.projectile = new Flame(_projectile, _bulletSpeed, _bulletLieftime);
        this.weapon = new Flamethrower(_weaponPrefab, _gunPoint, projectile, _maxAmmo, boostForceVertical, boostForceHorizontal);
        player = GameObject.FindWithTag("Player");
        playerRB = player.GetComponent<Rigidbody>();
    }

    public override void Shoot()
    {
        flame = Instantiate(projectile._projectilePrefab, weapon.gunPoint.transform);
    }

    public override void ShootHeld()
    {
        if (!weapon.expendsAmmo()) { destroyFlame(); return; }
        Vector3 horizVector = boostHorizontally();
        Vector3 vertVector = boostVertically();
        playerRB.AddForce(horizVector + vertVector, ForceMode.Force);
        Debug.DrawRay(player.transform.position, horizVector, Color.red);
        Debug.DrawRay(player.transform.position, vertVector);
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
        Vector3 forceVector =
            new Vector3(0, ((Flamethrower)weapon).getVerticalBoostStrength(playerRB.linearVelocity) * -1, 0);
        return forceVector;
    }

    private Vector3 boostHorizontally()
    {
        Vector3 forceVector =
            new Vector3(weapon.gunPoint.transform.up.x, 0, weapon.gunPoint.transform.up.z) * -1 * ((Flamethrower)weapon).getHotizontalBoostStrength();
        return forceVector;
    }
}

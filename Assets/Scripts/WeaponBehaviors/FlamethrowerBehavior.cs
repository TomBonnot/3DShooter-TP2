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
    private bool _isShooting;

    void Awake()
    {
        this.flamethrower = new Flamethrower(_weaponPrefab, _gunPoint, _maxAmmo, boostForceVertical, boostForceHorizontal);
        this.weapon = this.flamethrower;
        playerRB = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        _isShooting = false;
    }

    public override void Shoot()
    {
        _soundEmitter.Play();
        flame = Instantiate(_projectilePrefab, weapon.gunPoint.transform);
        _isShooting = true;
    }
    private void Propel()
    {
        // Hover boost
        if (!weapon.expendsAmmo()) { destroyFlame(); return; }
        Vector3 horizVector = boostHorizontally();
        Vector3 vertVector = boostVertically();
        playerRB.AddForce(horizVector + vertVector, ForceMode.Force);
    }

    void FixedUpdate()
    {
        if (!_isShooting) return;
        Propel();
    }

    public override void ReleaseShoot()
    {
        _soundEmitter.Stop();
        destroyFlame();
    }

    private void destroyFlame()
    {
        _isShooting = false;
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

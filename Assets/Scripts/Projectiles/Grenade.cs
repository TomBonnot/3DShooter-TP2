using UnityEngine;

public class Grenade : Projectile
{
    /** 
     This class contains the data about the grenade.
     **/
    public float _radiusExplosion { get; protected set; }
    public float _explosionForce { get; protected set; }
    public GameObject _explosion_prefab { get; protected set; }
    public Grenade(float velocity, float lifetime, float _radiusExplosion, float _explosionForce, GameObject _explosion_prefab)
    {
        this.velocity = velocity;
        this.projectileLifetime = lifetime;
        this._radiusExplosion = _radiusExplosion;
        this._explosionForce = _explosionForce;
        this._explosion_prefab = _explosion_prefab;
    }
}

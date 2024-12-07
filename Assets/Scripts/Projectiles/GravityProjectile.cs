using UnityEngine;

public class GravityProjectile : Projectile
{
    /** 
    This class contains the data about the gravity projectile.
    **/
    public float _radiusExplosion { get; protected set; }
    public GameObject _explosion_prefab {  get; protected set; }
    public GravityProjectile(float velocity, float lifetime, float _radiusExplosion, GameObject _explosion_prefab)
    {        
        this.velocity = velocity;
        this.projectileLifetime = lifetime;
        this._radiusExplosion = _radiusExplosion;
        this._explosion_prefab = _explosion_prefab;
    }
}

using UnityEngine;

public class GravityProjectile : Projectile
{
    /** 
    This class contains the data about the gravity projectile.
    **/
    public float _radiusExplosion { get; protected set; }
    public GravityProjectile(float velocity, float lifetime, float _radiusExplosion)
    {        
        this.velocity = velocity;
        this.projectileLifetime = lifetime;
        this._radiusExplosion = _radiusExplosion;
    }
}

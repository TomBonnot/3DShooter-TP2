using UnityEngine;

public class Gravity : Projectile
{
    /** 
    Ici on gère la logique interne d'un inverseur de gravité
    **/
    public float _radiusExplosion { get; protected set; }
    public Gravity(GameObject prefab, float velocity, float lifetime, float _radiusExplosion)
    {
        this._projectilePrefab = prefab;
        this.velocity = velocity;
        this.projectileLifetime = lifetime;
        this._radiusExplosion = _radiusExplosion;
    }
}

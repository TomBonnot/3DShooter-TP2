using UnityEngine;

public class Rocket : Projectile
{
    /** 
    Ici on gère la logique interne d'un Rocket
    Ex: Ce projectile accélère avec le temps à cause du rocket power
    **/

    public float explosionStrength { get; protected set; }
    public Rocket(GameObject prefab, float velocity, float lifetime, float explosionStrength)
    {
        this._projectilePrefab = prefab;
        this.velocity = velocity;
        this.projectileLifetime = lifetime;
        this.explosionStrength = explosionStrength;
    }
}

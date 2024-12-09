using UnityEngine;

public abstract class Projectile
{
    /** 
    Ici on gère la logique interne d'un projectile en général
    Ex: Tout projectile perd de la vélocité avec le temps à cause de la friction
    Il aura surement très peu de actual code dans ces fichiers
    **/
    public float velocity { get; protected set; }
    public float projectileLifetime { get; protected set; }
}
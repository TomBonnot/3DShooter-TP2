using UnityEngine;

public class Flame : Projectile
{
    /** 
    Ici on gère la logique interne d'un Rocket
    Ex: Ce projectile accélère avec le temps à cause du rocket power
    **/

    public Flame(float velocity, float lifetime)
    {
        this.velocity = velocity;
        this.projectileLifetime = lifetime;
    }
}

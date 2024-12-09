using UnityEngine;

public class BasicProjectile : Projectile
{
    /** 
    Ici on gère la logique interne d'un Rocket
    Ex: Ce projectile accélère avec le temps à cause du rocket power
    **/
    public BasicProjectile(float velocity)
    {
        this.velocity = velocity;
    }
}

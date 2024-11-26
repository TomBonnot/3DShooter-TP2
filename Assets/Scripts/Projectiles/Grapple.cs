using UnityEngine;

public class Grapple : Projectile
{
    /** 
    Ici on gère la logique interne d'un Rocket
    Ex: Ce projectile accélère avec le temps à cause du rocket power
    **/
    public Grapple(float velocity)
    {
        this.velocity = velocity;
    }
}

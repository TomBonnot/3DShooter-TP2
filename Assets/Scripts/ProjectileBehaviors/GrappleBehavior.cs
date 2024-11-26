using UnityEngine;
using System;

public class GrappleBehavior : ProjectileBehavior
{
    // Delegate et Hook pour pouvoir faire un callback quand on s'est attaché
    // à quelque chose de Grappleable
    public delegate void OnGrappleHooked(Vector3 collisionPoint);
    private OnGrappleHooked hookCallback;
    
    // On en a pas encore besoin, mais on respecte le pattern sur tous
    // les projectile behaviors
    private Grapple _grapple;

    public void setGrapple(Grapple g)
    {
        _grapple = g;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        // Si on collide avec quelque chose de Grappleable, le grapple ne bouge plus
        // Et on active le callback
        if (collision.gameObject.layer == Layers.GRAPPLEABLE)
        {
            Debug.Log("Get Grappled");
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            hookCallback(collision.GetContact(0).point);
        }
    }

    public void setGrappleEvent(OnGrappleHooked cb)
    {
        this.hookCallback = cb;
    }
}
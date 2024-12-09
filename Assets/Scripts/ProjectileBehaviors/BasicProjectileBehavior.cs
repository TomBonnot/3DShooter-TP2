using UnityEngine;
using System;

public class BasicProjectileBehavior : ProjectileBehavior
{
    /** Ici on gère le lifetime d'un Rocket **/

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("pew");
        }
    }
}
using UnityEngine;
using System;

public class RocketBehavior : ProjectileBehavior
{
    /** Ici on gère le lifetime d'un Rocket **/

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("bam");
        }
    }
}
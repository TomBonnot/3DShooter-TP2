using UnityEngine;
using System;

public class FlameBehavior : ProjectileBehavior
{
    /** Ici on gère le lifetime d'une flame **/

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("ground burns");
        }
    }
}
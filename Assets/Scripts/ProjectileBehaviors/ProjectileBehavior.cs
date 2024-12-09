using System;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    /** Ici on gère le lifetime d'un projectile en général **/
    protected Projectile _projectile;

    public virtual void SetProjectile(Projectile projectile)
    {
        this._projectile = projectile;
    }
}

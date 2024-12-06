using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeBehavior : ProjectileBehavior
{
   /** This class manages the behavior of the grenade **/

    // Data of the launched projectile
    Grenade _grenade;

    public void Explode()
    {
        // Disable the collider otherwise too many other collisions
        this.GetComponent<Collider>().enabled = false;

        // Find every GameObject in the gravity explosion
        Vector3 _explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(_explosionPosition, _grenade._radiusExplosion);

        foreach (Collider hit in colliders)
        {
            Rigidbody _rbHit = hit.GetComponent<Rigidbody>();

            if (_rbHit != null)
            {
                _rbHit.AddExplosionForce(_grenade._explosionForce, _explosionPosition, _grenade._radiusExplosion);
            }
        }

        Destroy(gameObject);
    }

    // Set the projectile to get the data
    public void SetProjectile(Grenade _projectile)
    {
        this._grenade = _projectile;
    }
    
}

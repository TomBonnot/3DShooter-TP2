//using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeBehavior : ProjectileBehavior
{
    /** This class manages the behavior of the grenade **/

    // Data of the launched projectile
    Grenade _grenade;

    public void Explode()
    {
        this.GetComponent<FMODUnity.StudioEventEmitter>().Play();
        // Disable the collider otherwise too many other collisions
        this.GetComponent<Collider>().enabled = false;

        // Find every GameObject in the explosion zone
        Vector3 _explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(_explosionPosition, _grenade._radiusExplosion);

        foreach (Collider hit in colliders)
        {
            Rigidbody _rbHit = hit.GetComponent<Rigidbody>();

            if (_rbHit != null)
            {
                // Apply explosion force to every objects in the zone
                _rbHit.AddExplosionForce(_grenade._explosionForce, _explosionPosition, _grenade._radiusExplosion);
            }

            // Check if the hit object is an enemy
            EnemyBehavior enemyBehavior = hit.GetComponent<EnemyBehavior>();

            if (enemyBehavior != null)
            {
                enemyBehavior.EnemyKilled();
            }


        }

        ParticleSystem _eplosionPrefab = Instantiate(_grenade._explosion_prefab, _explosionPosition, Quaternion.identity).GetComponent<ParticleSystem>();
        Destroy(gameObject);
    }

    // Set the projectile to get the data
    public void SetProjectile(Grenade _projectile)
    {
        this._grenade = _projectile;
    }

}

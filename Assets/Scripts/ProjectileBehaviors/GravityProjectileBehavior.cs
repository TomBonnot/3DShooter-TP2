using Tripolygon.UModelerX.Runtime.MessagePack.Resolvers;
using UnityEngine;

public class GravityProjectileBehavior : ProjectileBehavior
{
    /** This class manages the behavior of the gravity projectile which reverses the gravity of objects **/

    // Data of the launched projectile
    GravityProjectile _gravityProjectile;

    // 
    private void OnTriggerEnter(Collider other)
    {
        // Disable the collider otherwise too many other collisions
        this.GetComponent<Collider>().enabled = false;

        // Find every GameObject in the gravity explosion
        Vector3 _explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(_explosionPosition, _gravityProjectile._radiusExplosion);

        foreach (Collider hit in colliders)
        {
            AntiGravityObject _antiGravityObject = hit.GetComponent<AntiGravityObject>();
            AntiGravityPlayer _antiGravityPlayer = hit.GetComponent<AntiGravityPlayer>();
            AntiGravityEnemy _antiGravityEnemy = hit.GetComponent<AntiGravityEnemy>();

            // Check if the GameObject hit is the player or an object whose gravity can be changed
            if (_antiGravityObject != null)
            {
                // Change the gravity of the object
                _antiGravityObject.ChangeGravity();
            }
            else if (_antiGravityPlayer != null)
            {
                // Change the gravity of the player
                _antiGravityPlayer.ChangeGravity();
            }
            else if(_antiGravityEnemy != null)
            {
                _antiGravityEnemy.ChangeGravity();
            }
        }

        ParticleSystem _eplosionPrefab = Instantiate(_gravityProjectile._explosion_prefab, _explosionPosition, Quaternion.identity).GetComponent<ParticleSystem>();
        Destroy(gameObject);
    }

    // Set the projectile to get the data
    public void SetProjectile(GravityProjectile gravityProjectile)
    {
        this._gravityProjectile = gravityProjectile;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Vector3 _explosionPosition = transform.position;
    //    Collider[] colliders = Physics.OverlapSphere(_explosionPosition, _radius);

    //    foreach (Collider hit in colliders)
    //    {
    //        Rigidbody _rbHit = hit.GetComponent<Rigidbody>();
    //        AntiGravityObject _antiGravityObject = hit.GetComponent<AntiGravityObject>();

    //        if (_rbHit != null && _antiGravityObject != null)
    //        {
    //            _antiGravityObject.ChangeGravity();
    //        }

    //        //hit.gameObject.GetComponent<Renderer>().material.color = Color.red;


    //        //Rigidbody _rb = hit.GetComponent<Rigidbody>();

    //        //if (_rb != null)
    //        //{
    //        //    _rb.AddExplosionForce(_power, _explosionPosition, _radius, 3f);
    //        //}
    //    }
    //}

}

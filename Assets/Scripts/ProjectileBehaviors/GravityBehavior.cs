using Tripolygon.UModelerX.Runtime.MessagePack.Resolvers;
using UnityEngine;

public class GravityBehavior : ProjectileBehavior
{
    /** Ici on gère le lifetime d'un inverseur de gravité **/
    private float _radiusExplosion = 5f;
    private void OnTriggerEnter(Collider other)
    {
        
        Vector3 _explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(_explosionPosition, _radiusExplosion);

        foreach (Collider hit in colliders)
        {
            Rigidbody _rbHit = hit.GetComponent<Rigidbody>();
            AntiGravityObject _antiGravityObject = hit.GetComponent<AntiGravityObject>();
            AntiGravityPlayer _antiGravityPlayer = hit.GetComponent<AntiGravityPlayer>();

            if (_rbHit != null && (_antiGravityObject != null || _antiGravityPlayer != null))
            {                
                if(_antiGravityObject != null)
                {
                    _antiGravityObject.ChangeGravity();
                }
                else if(_antiGravityPlayer != null)
                {
                    _antiGravityPlayer.ChangeGravity(); 
                }
            }
        }

        Destroy(gameObject);
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

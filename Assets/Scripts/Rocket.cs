using UnityEngine;

public class Rocket : ProjectileBehavior
{
    protected override void OnCollisionEnter(Collision collision)
    {
        // Destroy(gameObject);
    }
}

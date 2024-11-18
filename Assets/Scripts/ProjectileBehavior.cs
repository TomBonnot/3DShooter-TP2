using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    //main timer, destroy the gameobject at the end of it
    public float _velocity { get; protected set; }
    public float projectileLifetime { get; protected set; }
    public GameObject _projectilePrefab { get; protected set; }

    /**
    *   Whenever there is a collision, spawn a decal and start a coroutine 
    **/
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject, projectile.projectileLifetime);
        }
    }

    protected void shootWeapon()
    {
        // bullet = Instantiate(projectile.prefab)
        // bullet.doStuffMaybe
    }
}

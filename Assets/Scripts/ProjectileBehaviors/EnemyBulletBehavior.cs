using UnityEngine;

public class EnemyBulletBehavior : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        Destroy(this.gameObject);
    }
}

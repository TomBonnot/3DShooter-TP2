using UnityEngine;

public class Bullet : MonoBehaviour
{
    //main timer, destroy the gameobject at the end of it
    [SerializeField] protected float _timer = 3f;
    protected virtual float Timer => _timer;

    /**
    *   Whenever there is a collision, spawn a decal and start a coroutine 
    **/
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject, _timer);
        }
    }
}

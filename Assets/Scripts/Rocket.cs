using UnityEngine;

public class Rocket : Bullet
{
    protected override float Timer => _timer;

    protected override void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, _timer);
    }
}

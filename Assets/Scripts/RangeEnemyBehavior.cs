using UnityEngine;

public class RangeEnemyBehavior : EnemyBehavior
{
    // Every variable about range attack
    private float reloadTime;
    private bool _attacked;
    void Update()
    {
        if (IsPlayerInSight())
        {
            AttackPlayer();
        }
    }

    protected override void AttackPlayer()
    {

    }
}

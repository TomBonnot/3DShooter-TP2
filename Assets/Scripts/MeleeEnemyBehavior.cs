using UnityEngine;

public class MeleeEnemyBehavior : EnemyBehavior
{
    [SerializeField] private float _moveSpeed;
    private Rigidbody _rb;

    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (IsPlayerInSight())
        {
            this.transform.LookAt(_player.transform);
            ChasePlayer();
        }
    }
    protected override void AttackPlayer()
    {

    }

    private void ChasePlayer()
    {
        Vector3 _targetDirection = (_player.transform.position - this.transform.position).normalized;

        Vector3 _velocity = new Vector3(_targetDirection.x * _moveSpeed, _rb.linearVelocity.y, _targetDirection.z * _moveSpeed);
        _rb.linearVelocity = _velocity;
    }
}

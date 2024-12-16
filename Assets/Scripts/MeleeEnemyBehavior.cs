using System.Collections;
using SingularityGroup.HotReload;
using UnityEngine;

public class MeleeEnemyBehavior : EnemyBehavior
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _cooldownInS;
    private Rigidbody _rb;
    private bool _canAttack;

    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
        _canAttack = true;
    }
    void Update()
    {
        if (IsPlayerInSight())
        {
            float distanceThisFrame = distanceToPlayer();
            this.transform.LookAt(_player.transform);
            // Debug.Log(distanceThisFrame + " " + _attackRange + " " + _canAttack);
            if (distanceThisFrame > _attackRange / 2) // pour éviter que l'ennemi se rapproche trop 
            {
                ChasePlayer();
            }
            else
            {
                StopChasingPlayer();
            }
            if (distanceThisFrame < _attackRange && _canAttack)
            {
                StartCoroutine(setAttackCooldown(_cooldownInS));
                AttackPlayer();
            }
        }
    }
    protected override void AttackPlayer()
    {
        _playerController.getAttacked();
    }

    private void ChasePlayer()
    {
        Vector3 _targetDirection = (_player.transform.position - this.transform.position).normalized;

        Vector3 _velocity = new Vector3(_targetDirection.x * _moveSpeed, _rb.linearVelocity.y, _targetDirection.z * _moveSpeed);
        _rb.linearVelocity = _velocity;
    }

    private void StopChasingPlayer()
    {
        _rb.linearVelocity = Vector3.zero;
    }


    IEnumerator setAttackCooldown(float secs)
    {
        _canAttack = false;
        yield return new WaitForSeconds(secs);
        _canAttack = true;
    }

    private float distanceToPlayer()
    {
        return Vector3.Distance(_player.transform.position, transform.position);
    }
}

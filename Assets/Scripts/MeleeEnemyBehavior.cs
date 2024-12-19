using System;
using System.Collections;
using SingularityGroup.HotReload;
using UnityEngine;

public class MeleeEnemyBehavior : EnemyBehavior
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _cooldownInS;
    [SerializeField] private float _pushStrength;
    [SerializeField] private float _verticalPushStrength;
    private Rigidbody _rb;
    private bool _canAttack;
    private Vector3 _targetDirection;

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
        Vector3 pushDirection = new(_targetDirection.x * _pushStrength, _targetDirection.y + _verticalPushStrength, _targetDirection.z * _pushStrength);
        Debug.Log(pushDirection);
        _playerController.GetRigidbody().AddForce(pushDirection);
        _playerController.getAttacked();
    }

    private void ChasePlayer()
    {
        _targetDirection = (_player.transform.position - this.transform.position).normalized;

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

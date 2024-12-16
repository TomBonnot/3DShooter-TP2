using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBehavior : EntityBehavior
{
    protected GameObject _player;
    protected bool _playerJustSpotted;
    protected virtual void Start()
    {
        _player = GameObject.Find("Player");
        _playerJustSpotted = false;
    }
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Die();            
        }
    }

    protected bool IsPlayerInSight()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, (_player.transform.position - transform.position), out hit, Mathf.Infinity))
        {
            if(hit.transform.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    public void EnemyKilled()
    {
        Die();
    }

    protected override void Die()
    {
        GameManager.Instance.RegisterEnemyKill();
        base.Die();
    }

    protected abstract void AttackPlayer();

}

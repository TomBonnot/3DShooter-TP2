using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBehavior : EntityBehavior
{
    protected GameObject _player;
    protected Controller _playerController;
    protected bool _playerJustSpotted;
    protected virtual void Start()
    {
        _player = GameObject.Find("Player");
        _playerController = _player.GetComponent<Controller>();
        _playerJustSpotted = false;
    }
    protected void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.tag == "Player")
        // {
        //     Die();            
        // }
    }

    protected bool IsPlayerInSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (_player.transform.position - transform.position), out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    protected abstract void AttackPlayer();

}

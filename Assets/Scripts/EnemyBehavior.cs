using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBehavior : EntityBehavior
{
    protected GameObject _player;
    protected Controller _playerController;
    protected bool _playerJustSpotted;
    [SerializeField] float _visionRange;
    protected virtual void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<Controller>();
        _playerJustSpotted = false;
    }
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Die();            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fall")
        {
            Die();
        }
    }

    protected bool IsPlayerInSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (_player.transform.position - transform.position), out hit, _visionRange))
        {
            if (hit.transform.gameObject.tag == "Player")
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

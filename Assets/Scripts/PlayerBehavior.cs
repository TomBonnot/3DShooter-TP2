using UnityEngine;

public class PlayerBehavior : EntityBehavior
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "End")
        {
            GameManager.Instance.LevelCompleted();
        }
        else if(other.gameObject.tag == "Fall")
        {
            Die();
        }
    }

    protected override void Die()
    {
        GameManager.Instance.GameOver();
    }

}

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

    protected override void Die()
    {
        GameManager.Instance.GameOver();
    }

}

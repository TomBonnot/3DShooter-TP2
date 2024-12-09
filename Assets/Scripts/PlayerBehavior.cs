using UnityEngine;

public class PlayerBehavior : EntityBehavior
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameManager.Instance.GameOver();
            //Destroy(gameObject);
        }
    }
    
}
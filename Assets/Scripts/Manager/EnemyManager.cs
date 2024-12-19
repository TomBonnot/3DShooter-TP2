using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    /** This class reset the state of enemies when we restart a level **/

    // A list to store all enemy GameObjects in the scene
    private List<GameObject> enemies;

    void Start()
    {
        // Initialize the list of enemies by finding all GameObjects tagged as "Enemy"
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        // Subscribe to the OnReloadEnemies event from the GameManager
        // This will allow the manager to reset enemies when the event is triggered
        GameManager.Instance.OnReloadEnemies += ResetEnemies;
    }

    /**
    *   Reactivate all enemy GameObjects in the scene
    **/
    private void ResetEnemies()
    {
        // Reactivate all enemies
        foreach (var enemy in enemies)
        {
            enemy.SetActive(true);
            // If you want to reset other properties (like health, position)
            // enemy.GetComponent<Enemy>().ResetState();
        }
    }

    /**
    *   Unsubscribe from the OnReloadEnemies event to prevent memory leaks or null reference errors
    **/
    private void OnDisable()
    {
        GameManager.Instance.OnReloadEnemies -= ResetEnemies;
    }
}

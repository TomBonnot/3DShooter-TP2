using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<GameObject> enemies;

    void Start()
    {
        // Get all enemies at the start of the scene
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        GameManager.Instance.OnReloadEnemies += ResetEnemies;
    }

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

    private void OnDisable()
    {
        GameManager.Instance.OnReloadEnemies -= ResetEnemies;
    }
}

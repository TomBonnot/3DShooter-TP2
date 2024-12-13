using UnityEngine;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour
{
    /** 
    The GameManagerUI class handles the user interface elements related
    to game state changes, such as displaying game over panels and
    managing interactions like retrying the level.
   **/


    private GameManager _gameManager;
    [SerializeField] private GameObject _deadPanel;

    private void Start()
    {
        _gameManager = GameManager.Instance;

        // Subscribe to the OnGameOver event to activate the death panel
        _gameManager.OnGameOver += ActiveDeadPanel;
    }

    // Displays the panel with the score and the retry button
    private void ActiveDeadPanel()
    {
        _deadPanel.SetActive(true);
    }

    // Method called when the "Retry" button is clicked
    public void RetryButton()
    {
        _gameManager.RestartLevel();
    }

    private void OnDisable()
    {
        _gameManager.OnGameOver -= ActiveDeadPanel;
    }

}
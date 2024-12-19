using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour
{
    /** 
    The GameManagerUI class handles the user interface elements related
    to game state changes, such as displaying game over panels and
    managing interactions like retrying the level.
   **/

    private GameManager _gameManager;

    [Header("GameObjects displayed in UI")]
    [SerializeField] private GameObject _deadPanel;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _optionMenu;
    [SerializeField] private GameObject _levelCompleted;
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private TMP_Text _scoreGameOver;
    [SerializeField] private TMP_Text _scoreLevelCompleted;

    private void Start()
    {
        _gameManager = GameManager.Instance;

        // Subscribe to various GameManager events to update the UI appropriately
        _gameManager.OnGameOver += ActiveDeadPanel;                  // Show the death panel when the game is over
        _gameManager.OnGamePaused += DisplayPauseMenu;               // Show or hide the pause menu
        _gameManager.OnUpdateTimer += UpdateTimer;                   // Update the timer display
        _gameManager.OnReloadLevel += CloseDeadPanel;                // Hide the death panel when reloading the level
        _gameManager.OnScoreUpdated += UpdateScore;                  // Update the score on Game Over
        _gameManager.OnLevelCompleted += DisplayLevelCompletedPanel; // Show the level completed panel
    }

    /**
    *   Displays or hides the level completed panel and updates the score text
    **/
    private void DisplayLevelCompletedPanel(bool _state, int _totalScore)
    {
        _levelCompleted.SetActive(_state);
        _scoreLevelCompleted.text = "Score : " + _totalScore;
    }

    /**
    *   Updates the score displayed on the Game Over panel
    **/
    private void UpdateScore(int _totalScore)
    {
        _scoreGameOver.text = "Score : " + _totalScore;
    }

    /**
    *   Activates the Game Over panel
    **/
    private void ActiveDeadPanel()
    {
        _deadPanel.SetActive(true);        
        
    }

    /**
    *   Deactivates the Game Over panel
    **/
    private void CloseDeadPanel()
    {
        _deadPanel.SetActive(false);
    }

    /**
    *   Displays or hides the pause menu and options menu based on the given state
    **/
    private void DisplayPauseMenu(bool _state)
    {
        _pauseMenu.SetActive(_state);
        _optionMenu.SetActive(_state);        
    }

    /**
    *   Updates the timer text in the UI
    **/
    private void UpdateTimer(string _time)
    {
        _timer.text = _time;
    }

    /**
    *   Unsubscribe from GameManager events to avoid memory leaks
    **/
    private void OnDisable()
    {
        _gameManager.OnGameOver -= ActiveDeadPanel;
        _gameManager.OnGamePaused -= DisplayPauseMenu;
        _gameManager.OnUpdateTimer -= UpdateTimer;
        _gameManager.OnReloadLevel -= CloseDeadPanel;
       _gameManager.OnScoreUpdated -= UpdateScore;
        _gameManager.OnLevelCompleted -= DisplayLevelCompletedPanel;
    }
}

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

        // Subscribe to the OnGameOver event to activate the death panel
        _gameManager.OnGameOver += ActiveDeadPanel;
        _gameManager.OnGamePaused += DisplayPauseMenu;
        _gameManager.OnUpdateTimer += UpdateTimer;
        _gameManager.OnReloadLevel += CloseDeadPanel;
        _gameManager.OnScoreUpdated += UpdateScore;
        _gameManager.OnLevelCompleted += DisplayLevelCompletedPanel;
    }

    private void DisplayLevelCompletedPanel(bool _state, int _totalScore)
    {
        _levelCompleted.SetActive(_state);
        _scoreLevelCompleted.text = "Score : " + _totalScore;
    }

    private void UpdateScore(int _totalScore)
    {
        _scoreGameOver.text = "Score : " + _totalScore;
    }

    // Displays the panel with the score and the retry button
    private void ActiveDeadPanel()
    {
        _deadPanel.SetActive(true);        
        
    }
    private void CloseDeadPanel()
    {
        _deadPanel.SetActive(false);
    }

    private void DisplayPauseMenu(bool _state)
    {
        _pauseMenu.SetActive(_state);
        _optionMenu.SetActive(_state);        
    }

    private void UpdateTimer(string _time)
    {
        _timer.text = _time;
    }

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

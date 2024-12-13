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

    private void Start()
    {
        _gameManager = GameManager.Instance;

        // Subscribe to the OnGameOver event to activate the death panel
        _gameManager.OnGameOver += ActiveDeadPanel;
        _gameManager.OnGamePaused += DisplayPauseMenu;
    }

    // Displays the panel with the score and the retry button
    private void ActiveDeadPanel()
    {
        _deadPanel.SetActive(true);
    }

    private void DisplayPauseMenu(bool _state)
    {
        _pauseMenu.SetActive(_state);
        _optionMenu.SetActive(_state);        
    }

    private void OnDisable()
    {
        _gameManager.OnGameOver -= ActiveDeadPanel;
        _gameManager.OnGamePaused -= DisplayPauseMenu;
    }



}

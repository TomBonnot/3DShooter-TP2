using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action OnGameOver;
    public event Action<bool> OnGamePaused;
    private string _selectedLevel;
    private bool _isGamePaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _isGamePaused = false;
    }

    // The player is dead
    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OnGameOver?.Invoke();
        Time.timeScale = 0f;
    }

    // Restart the current level
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void LoadScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        _isGamePaused = !_isGamePaused;
        if (_isGamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        OnGamePaused?.Invoke(_isGamePaused);
    }
}

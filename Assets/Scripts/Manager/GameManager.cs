using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /** The GameManager class handles game state management **/

    // Singleton instance of the GameManager
    public static GameManager Instance { get; private set; }

    // Events for broadcasting game state changes to other components
    public event Action OnGameOver;                      // Triggered when the player dies
    public event Action<bool> OnGamePaused;              // Triggered when the game is paused/unpaused
    public event Action OnEnableDisableControllerPlayer; // Triggered to enable/disable player controls
    public event Action OnStartCountDown;                // Triggered when a countdown starts
    public event Action<string> OnUpdateTimer;           // Triggered to update the timer display
    public event Action<int> OnScoreUpdated;             // Triggered when the score is updated
    public event Action OnReloadLevel;                   // Triggered to reload the level
    public event Action OnReloadEnemies;                 // Triggered to reload enemy states
    public event Action<bool, int> OnLevelCompleted;     // Triggered when a level is completed

    // Reference to the player GameObject
    private GameObject player;

    // Internal state variables
    private bool _isGamePaused;
    private bool _isPlaying;
    private bool _firstTime;
    private float _elapsedTime;
    private TimeSpan _timePlaying;
    private Coroutine _timerCoroutine;  // Add this field to track the timer coroutine

    [Header("Score")]
    private int _playerKills;
    private int _totalScore;
    private float _baseTimeMultiplier;
    private int _pointsPerKill;
    private int _timeMultiplierFactor;

    

    private void Awake()
    {
        // Singleton pattern implementation to ensure only one GameManager exists
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Duplicate GameManager found. Destroying new instance.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _playerKills = 0;
        _totalScore = 0;
        _baseTimeMultiplier = 10f;
        _pointsPerKill = 100;
        _timeMultiplierFactor = 10;
        
        _isGamePaused = false;
        _isPlaying = false;
        _firstTime = true;
        CheckForPlayer();

        // If the player is in the scene and this is the first time, start the countdown
        if (IsPlayerInScene() && _firstTime)
        {
            _elapsedTime = 0f;
            OnEnableDisableControllerPlayer?.Invoke();
            OnStartCountDown?.Invoke();
        }
    }

    /**
    *   Find if the player is in the scene
    **/
    private void CheckForPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /**
    *   Is the player in the scene
    **/
    private bool IsPlayerInScene()
    {
        return player != null;
    }

    /**
    *   Handles the game over state when the player dies
    **/
    public void GameOver()
    {
        _isPlaying = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OnGameOver?.Invoke();
        // Your score is 0 when you die
        OnScoreUpdated?.Invoke(0);
        // Pause the game
        Time.timeScale = 0f;
    }

    /**
    *   Restarts the current level and resets game variables
    **/
    public void RestartLevel()
    {
        // Stop existing timer coroutine if it exists
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        _isPlaying = false;  // Ensure we stop any existing timer
        _playerKills = 0;
        _totalScore = 0;
        _elapsedTime = 0f;

        OnReloadEnemies?.Invoke();
        OnLevelCompleted?.Invoke(false, 0);

        OnEnableDisableControllerPlayer?.Invoke();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        OnReloadLevel?.Invoke();

        BeginGame();
    }

    /**
    *   Loads a specified scene
    **/
    public void LoadScene(string _sceneName)
    {
        if(Instance == this)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_sceneName);
        }
        
    }

    /**
    *   Toggles the paused state of the game
    **/
    public void PauseGame()
    {
        _isGamePaused = !_isGamePaused;       

        if (_isGamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // Pause the game
            Time.timeScale = 0f; 
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // Resume the game
            Time.timeScale = 1f;

            
        }
        OnGamePaused?.Invoke(_isGamePaused);
    }

    /**
    *   Begins the game and starts the timer
    **/
    public void BeginGame()
    {
        _firstTime = false;
        OnEnableDisableControllerPlayer?.Invoke();
        _isPlaying = true;

        // Stop any existing timer coroutine
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }
        _timerCoroutine = StartCoroutine(UpdateTimer());
    }

    /**
    *   Resume the game
    **/
    //private void Playing()
    //{
    //    // Stop any existing timer coroutine
    //    if (_timerCoroutine != null)
    //    {
    //        StopCoroutine(_timerCoroutine);
    //    }
    //    _timerCoroutine = StartCoroutine(UpdateTimer());
    //}

    /**
    *   Updates the timer while the game is in progress
    **/
    private IEnumerator UpdateTimer()
    {
        while (_isPlaying)
        {
            if (!_isGamePaused)  // Only update time if game is not paused
            {
                _elapsedTime += Time.deltaTime;
                _timePlaying = TimeSpan.FromSeconds(_elapsedTime);
                OnUpdateTimer?.Invoke(_timePlaying.ToString("mm':'ss'.'ff"));
            }
            yield return null;
        }
    }


    /**
    *   Tracks the number of enemies killed by the player
    **/
    public void RegisterEnemyKill()
    {
        _playerKills++;
    }

    /**
    *   Handles actions when the level is completed
    **/
    public void LevelCompleted()
    {
        CalculateScore();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    /**
    *   Calculates the player's score based on kills and time
    **/
    private void CalculateScore()
    {
        // Calculate score based on time and kills
        // Lower time means higher score
        float timeMultiplier = Mathf.Max(1f, _baseTimeMultiplier - _elapsedTime);
        int killBonus = _playerKills * _pointsPerKill;

        _totalScore = Mathf.RoundToInt(killBonus + (timeMultiplier * _timeMultiplierFactor));

        OnLevelCompleted?.Invoke(true, _totalScore);
    }

    /**
    *   Check for the player GameObject after the scene is loaded
    **/
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckForPlayer();
    }

    /**
    *    Quit the game
    **/
    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

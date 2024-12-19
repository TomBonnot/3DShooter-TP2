using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnGameOver;
    public event Action<bool> OnGamePaused;
    public event Action OnEnableDisableControllerPlayer;
    public event Action OnStartCountDown;
    public event Action<string> OnUpdateTimer;    
    public event Action<int> OnScoreUpdated;
    public event Action OnReloadLevel;
    public event Action OnReloadEnemies;
    public event Action<bool, int> OnLevelCompleted;

    private GameObject player;

    private bool _isGamePaused;
    private bool _isPlaying;
    private bool _firstTime;
    private float _elapsedTime;
    private TimeSpan _timePlaying;

    [Header("Score")]
    private int _playerKills;
    private int _totalScore;
    private float _baseTimeMultiplier;
    private int _pointsPerKill;
    private int _timeMultiplierFactor;

    private Coroutine _timerCoroutine;  // Add this field to track the timer coroutine

    private void Awake()
    {

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
        if (IsPlayerInScene() && _firstTime)
        {
            _elapsedTime = 0f;
            OnEnableDisableControllerPlayer?.Invoke();
            OnStartCountDown?.Invoke();
        }
    }

    private void CheckForPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private bool IsPlayerInScene()
    {
        return player != null;
    }

    // The player is dead
    public void GameOver()
    {
        _isPlaying = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OnGameOver?.Invoke();
        // Your score is 0 when you die
        OnScoreUpdated?.Invoke(0);
        Time.timeScale = 0f;
    }

    // Restart the current level
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

    // Load the scene
    public void LoadScene(string _sceneName)
    {
        if(Instance == this)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_sceneName);
        }
        
    }
    public void PauseGame()
    {
        _isGamePaused = !_isGamePaused;       

        if (_isGamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;

            
        }
        OnGamePaused?.Invoke(_isGamePaused);
    }

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

    private void Playing()
    {
        // Stop any existing timer coroutine
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }
        _timerCoroutine = StartCoroutine(UpdateTimer());
    }

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


    // Save the number of enemies killed
    public void RegisterEnemyKill()
    {
        _playerKills++;
    }

    public void LevelCompleted()
    {
        CalculateScore();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    private void CalculateScore()
    {
        // Calculate score based on time and kills
        // Lower time means higher score
        float timeMultiplier = Mathf.Max(1f, _baseTimeMultiplier - _elapsedTime);
        int killBonus = _playerKills * _pointsPerKill; // 100 points per kill

        _totalScore = Mathf.RoundToInt(killBonus + (timeMultiplier * _timeMultiplierFactor));

        //OnScoreUpdated?.Invoke(_totalScore);
        OnLevelCompleted?.Invoke(true, _totalScore);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckForPlayer();
    }

    // Quit the game
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

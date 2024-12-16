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
    public event Action OnReloadLevel;
    public event Action<int> OnScoreUpdated;
    public event Action OnReloadEnemies;

    private GameObject player;

    private bool _isGamePaused;
    private bool _isPlaying;
    private bool _firstTime;
    private bool _isDead;
    private float _elapsedTime;
    private TimeSpan _timePlaying;

    [Header("Score")]
    private int _playerKills;
    private int _totalScore;


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
        _isDead = false;
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

    // The player is dead
    public void GameOver()
    {
        _isPlaying = false;
        _isDead = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OnGameOver?.Invoke();
        CalculateScore();
        Time.timeScale = 0f;
    }

    // Restart the current level
    public void RestartLevel()
    {
        _playerKills = 0;
        _totalScore = 0;
        OnReloadEnemies?.Invoke();
        OnScoreUpdated?.Invoke(0);
        OnEnableDisableControllerPlayer?.Invoke();
        Time.timeScale = 1f;
        OnReloadLevel?.Invoke();
        BeginGame();
        _elapsedTime = 0f;
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

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }

    // Pause and unpause the game
    public void PauseGame()
    {
        _isGamePaused = !_isGamePaused;

        // Only change _isPlaying if BeginGame() has been called (when _firstTime is false)
        if (!_firstTime)
        {
            _isPlaying = !_isGamePaused;
        }

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

            // Only call Playing() if BeginGame() has been called
            if (!_firstTime)
            {
                Playing();
            }
        }
        OnGamePaused?.Invoke(_isGamePaused);
    }

    private void CheckForPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private bool IsPlayerInScene()
    {
        return player != null;
    }

    public void BeginGame()
    {
        _firstTime = false;
        OnEnableDisableControllerPlayer?.Invoke();
        _isPlaying = true;
        StartCoroutine(UpdateTimer());
    }

    private void Playing()
    {
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (_isPlaying)
        {
            _elapsedTime += Time.deltaTime;
            _timePlaying = TimeSpan.FromSeconds(_elapsedTime);
            OnUpdateTimer?.Invoke(_timePlaying.ToString("mm':'ss'.'ff"));
            yield return null;
        }
    }

    public void RegisterEnemyKill()
    {
        _playerKills++;
        CalculateScore();
    }

    private void CalculateScore()
    {
        // Calculate score based on time and kills
        // Lower time means higher score
        float timeMultiplier = Mathf.Max(1f, 10f - _elapsedTime);
        int killBonus = _playerKills * 100; // 100 points per kill

        _totalScore = Mathf.RoundToInt(killBonus + (timeMultiplier * 10));

        OnScoreUpdated?.Invoke(_totalScore);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckForPlayer();
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

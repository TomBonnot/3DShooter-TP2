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
    public event Action<string> OnStartTimer;
    private GameObject player;

    private bool _isRestarting;
    private bool _isGamePaused;
    private bool _isPlaying;
    private bool _firstTime;
    private float _elapsedTime;

    private TimeSpan _timePlaying;
    

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
        _isRestarting = false;
        _isGamePaused = false;
        _isPlaying = false;
        _firstTime = true;
        CheckForPlayer();
        
        if (IsPlayerInScene() && _firstTime)
        {
            
            _elapsedTime = 0f;
            OnEnableDisableControllerPlayer?.Invoke();
            OnStartCountDown?.Invoke();
            _firstTime = false;
        }
        else if(IsPlayerInScene())
        {
            _elapsedTime = 0f;
            _isPlaying = true;
            Playing();
        }
    }

    // The player is dead
    public void GameOver()
    {
        _isPlaying = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OnGameOver?.Invoke();
        Time.timeScale = 0f;
    }

    // Restart the current level
    public void RestartLevel()
    {
        //Debug.Log("RestartLevel called at: " + Time.time);
        //Time.timeScale = 1f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Load the scene
    public void LoadScene(string _sceneName)
    {
        if(Instance == this)
        {
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
        _isPlaying = _isGamePaused;
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
            OnStartTimer?.Invoke(_timePlaying.ToString("mm':'ss'.'ff"));
            yield return null;
        }
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("Let's Go!");
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

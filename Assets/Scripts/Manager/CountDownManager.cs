using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class CountDownManager : MonoBehaviour
{
    /**
    *   This class manage the countdown timer at the start of the level
    **/

    [SerializeField] private TMP_Text _countDownTimer;
    private int _countDownTime;

    void Start()
    {
        _countDownTime = 3;
        GameManager.Instance.OnStartCountDown += StartCountDown;        
    }

    /**
    *   Start the countdown coroutine when the event is triggered
    **/
    private void StartCountDown()
    {
        StartCoroutine(CountDown());
    }

    /**
    *   Manage the count down
    **/
    private IEnumerator CountDown()
    {
        while (_countDownTime > 0)
        {
            // Display the current countdown time
            DisplayCountDown(_countDownTime.ToString());
            yield return new WaitForSeconds(1f);
            _countDownTime--;
        }
        // When the countdown finishes, signal the GameManager to begin the game
        GameManager.Instance.BeginGame();
        DisplayCountDown("Go!");

        yield return new WaitForSeconds(1f);
        // Clear the countdown display
        DisplayCountDown("");
    }

    /**
    *   Update the countdown text displayed in the UI
    **/
    public void DisplayCountDown(string time)
    {
        _countDownTimer.text = time;
    }

    /**
    *   Unsubscribe from the OnStartCountDown event when the script is disabled
    **/
    private void OnDisable()
    {
        GameManager.Instance.OnStartCountDown -= StartCountDown;
    }
}

using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class CountDownManager : MonoBehaviour
{

    [SerializeField] private TMP_Text _countDownTimer;
    private int _countDownTime;

    void Start()
    {
        _countDownTime = 3;
        GameManager.Instance.OnStartCountDown += StartCountDown;        
    }

    private void StartCountDown()
    {
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        while (_countDownTime > 0)
        {
            DisplayCountDown(_countDownTime.ToString());
            yield return new WaitForSeconds(1f);
            _countDownTime--;
        }
        GameManager.Instance.BeginGame();
        DisplayCountDown("Go!");

        yield return new WaitForSeconds(1f);
        DisplayCountDown("");
    }

    public void DisplayCountDown(string time)
    {
        _countDownTimer.text = time;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStartCountDown -= StartCountDown;
    }
}

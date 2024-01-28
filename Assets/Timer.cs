using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer;
    private int minutes;
    private string seconds;
    public GameObject manager;

    public TextMeshProUGUI timerUI;
    public TextMeshProUGUI gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        // set timer to X minutes
        timer = 90;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        CheckGameOver();
    }

    private void UpdateTimer()
    {
        // update race time
        timer -= Time.deltaTime;
        minutes = ((int)timer / 60);
        seconds = (timer % 60).ToString("F0");
        timerUI.SetText($"{minutes}:{seconds}");
    }
    private void CheckGameOver()
    {
        if (timer <= 0)
        {
            //game over
            gameOverText.SetText($"GAME OVER");
            Debug.Log("game over");
        }
    }
}

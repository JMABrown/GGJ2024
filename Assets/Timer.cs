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

    public TextMeshProUGUI timerUI;

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
    }

    private void UpdateTimer()
    {
        // update race time
        timer -= Time.deltaTime;
        minutes = ((int)timer / 60);
        seconds = (timer % 60).ToString("F0");

        // use minutes/seconds to display time in UI
        if (minutes == 0)
        {
            //timerUI.text = seconds;
            timerUI.SetText($"{seconds}");
        }
        else
        {
            //timerUI.text = (minutes + ":" + seconds).ToString();
            timerUI.SetText($"{minutes}:{seconds}");
        }


    }
}

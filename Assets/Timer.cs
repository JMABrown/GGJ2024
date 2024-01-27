using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer;
    private int minutes;
    private string seconds; 

    // Start is called before the first frame update
    void Start()
    {
        // set timer to X minutes

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateTimer()
    {
        // update race time
        timer -= Time.deltaTime;
        minutes = ((int)timer / 60);
        seconds = (timer % 60).ToString("F3");

        // use minutes/seconds to display time in UI
        if (minutes == 0)
        {
            //timerUI.text = seconds;
        }
        else
        {
            //timerUI.text = (minutes + ":" + seconds).ToString();
        }


    }
}

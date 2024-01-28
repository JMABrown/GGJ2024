using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCanvas : MonoBehaviour
{
    public TextMeshProUGUI ScoreDisplay;
    public TextMeshProUGUI ScoreBufferDisplay;

    private float scoreBuffer;
    private float scoreToDisplay;

    private float scoreChunk = 555f;

    private float timeToAnimateBuffer;
    
    private float displayBufferPeriod = 1f;

    private void Update()
    {
        if (scoreBuffer > 0)
        {
            ScoreBufferDisplay.enabled = true;
        }
        else
        {
            ScoreBufferDisplay.enabled = false;
            return;
        }
        
        ScoreBufferDisplay.SetText($"+{scoreBuffer.ToString("F0")}");

        if (Time.time >= timeToAnimateBuffer)
        {
            if (scoreBuffer < scoreChunk)
            {
                scoreToDisplay += scoreBuffer;
                scoreBuffer = 0;
            }
            else
            {
                scoreToDisplay += scoreChunk;
                scoreBuffer -= scoreChunk;
            }
        }

        ScoreDisplay.SetText($"Score: {scoreToDisplay.ToString("F0")}");
        ScoreBufferDisplay.SetText($"+{scoreBuffer.ToString("F0")}");
    }

    private void Start()
    {
        DamageManager.OnDamageReported.RemoveListener(HandleDamageUpdate);
        DamageManager.OnDamageReported.AddListener(HandleDamageUpdate);
        ScoreDisplay.SetText($"Score: {scoreToDisplay}");
        ScoreBufferDisplay.enabled = false;
    }

    private void HandleDamageUpdate(DamageInfo damage)
    {
        scoreBuffer += damage.ImpactSquareMagnitude;

        timeToAnimateBuffer = Time.time + displayBufferPeriod;
    }
}

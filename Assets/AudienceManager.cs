using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudienceManager : MonoSingleton<AudienceManager>
{
    public float LaughLevel;
    
    public float MaxLaughLevel;

    public float DecayRate;

    public float LaughIncreaseFactor;

    public float LaughAsFraction => LaughLevel / MaxLaughLevel;

    public TextMeshProUGUI gameOverText;

    private void Start()
    {
        LaughLevel = MaxLaughLevel;
        
        DamageManager.OnDamageReported.RemoveListener(HandleDamageUpdate);
        DamageManager.OnDamageReported.AddListener(HandleDamageUpdate);
    }

    private void Update()
    {
        LaughLevel -= DecayRate;
        CheckGameOver();
    }
    
    private void HandleDamageUpdate(DamageInfo damage)
    {
        LaughLevel += LaughIncreaseFactor * damage.ImpactSquareMagnitude;
        LaughLevel = Mathf.Min(LaughLevel, MaxLaughLevel);
    }

    private void CheckGameOver()
    {
        if (LaughLevel <= 0)
        {
            //game over
            gameOverText.SetText($"GAME OVER");
            Debug.Log("game over");
        }
    }
}

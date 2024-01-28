using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AlcoholManager : MonoSingleton<AlcoholManager>
{
    public float AlcoholLevel = 0f;

    public Transform SpawnTarget;

    public GameObject DrinkPrefabSpawn;

    public float AlcoholPercentage => AlcoholLevel / MaxLevel;

    private float MaxLevel = 5f;

    private float DecayRate = 0.0002f;

    private float DrinkIncrease = 1f;

    private void Update()
    {
        AlcoholLevel -= DecayRate;
        AlcoholLevel = Mathf.Min(AlcoholLevel, MaxLevel);
        AlcoholLevel = Mathf.Max(AlcoholLevel, 0);

        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDrink();
        }
    }

    private void TakeDrink()
    {
        AlcoholLevel += DrinkIncrease;
        if (DrinkPrefabSpawn != null)
        {
            Instantiate(DrinkPrefabSpawn, SpawnTarget.position + SpawnTarget.forward, SpawnTarget.rotation);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageManager : MonoSingleton<DamageManager>
{
    public static UnityEvent<float> OnDamageReported = new UnityEvent<float>();
    
    private static List<DamageSensor> damageSensors = new List<DamageSensor>();

    private float latestDamage;

    private void Update()
    {
        if (latestDamage > 0)
        {
            OnDamageReported?.Invoke(latestDamage);
            latestDamage = 0;
        }
    }

    public void SubscribeSensor(DamageSensor newSensor)
    {
        damageSensors.Add(newSensor);
    }

    public void ReportDamage(float damage)
    {
        Debug.Log("Damage reported: " + damage);
        latestDamage += damage;
    }

    public float GetLatestTotalDamage()
    {
        return latestDamage;
    }
}

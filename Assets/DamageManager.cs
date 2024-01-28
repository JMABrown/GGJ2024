using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageManager : MonoSingleton<DamageManager>
{
    public static UnityEvent<DamageInfo> OnDamageReported = new UnityEvent<DamageInfo>();
    
    private static List<DamageSensor> damageSensors = new List<DamageSensor>();

    private List<DamageInfo> latestDamage = new List<DamageInfo>();

    private void Update()
    {
        if (latestDamage.Count > 0)
        {
            var latestTotalDamage = GetLatestTotalDamage();
            OnDamageReported?.Invoke(latestTotalDamage);
            Debug.Log(latestTotalDamage.Impulse);
            latestDamage.Clear();
        }
    }

    public void SubscribeSensor(DamageSensor newSensor)
    {
        damageSensors.Add(newSensor);
    }

    public void ReportDamage(DamageInfo damage)
    {
        latestDamage.Add(damage);
    }

    public DamageInfo GetLatestTotalDamage()
    {
        var latestTotalDamage = new DamageInfo();
        foreach (var damage in latestDamage)
        {
            latestTotalDamage.Impulse += damage.Impulse;
            latestTotalDamage.ImpactSquareMagnitude += damage.ImpactSquareMagnitude * damage.Weight;
        }
        return latestTotalDamage;
    }
}

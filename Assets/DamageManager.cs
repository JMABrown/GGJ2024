using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoSingleton<DamageManager>
{
    private static List<DamageSensor> damageSensors = new List<DamageSensor>();

    private float latestDamage;

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

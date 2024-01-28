using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
    public float ImpactSquareMagnitude = 0f;
    public Vector3 Impulse = Vector3.zero;
    public float Weight = 1f;
}

public class DamageSensor : MonoBehaviour
{

    public float SensorWeight = 1f;
    
    public bool Log = false;
    
    private static float minImpulseRequired = 15f;

    private DamageManager damageManger;

    private void Awake()
    {
        damageManger = DamageManager.Instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var impactSquareMagnitude = collision.impulse.sqrMagnitude;
        if (impactSquareMagnitude < minImpulseRequired)
        {
            return;
        }

        if (Log)
        {
            Debug.Log(collision.impulse);
            Debug.Log(collision.relativeVelocity);
            Debug.Log(collision.GetContact(0).separation);
        }

        var reportedDamage = new DamageInfo()
        {
            ImpactSquareMagnitude = impactSquareMagnitude,
            Impulse = collision.impulse,
            Weight = SensorWeight
        };

        damageManger.ReportDamage(reportedDamage);
    }
}

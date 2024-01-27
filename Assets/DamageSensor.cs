using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSensor : MonoBehaviour
{
    public float Weight = 1f;
    
    public bool Log;
    
    private static float minImpulseRequired = 5f;

    private DamageManager damageManger;

    private void Awake()
    {
        //DamageManager.Instance.SubscribeSensor(this);
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

        damageManger.ReportDamage(impactSquareMagnitude * Weight);
    }
}

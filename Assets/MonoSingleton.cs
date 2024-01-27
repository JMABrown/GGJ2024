using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                var newObject = new GameObject(typeof(T).ToString(), typeof(T));
                instance = newObject.GetComponent<T>();
            }
            return instance;
        }
    }

    protected static T instance;

    private void Awake()
    {
        instance = this.GetComponent<T>();
    }
}

using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T GetInstance(bool shouldCreate = true)
    {
        if (_instance == null)
        {
            _instance = GameObject.FindAnyObjectByType<T>();
        }

        if (_instance == null)
        {
            Debug.LogWarning("Could not find " + typeof(T));
            if (shouldCreate)
            {
                var newInstance = new GameObject();
                newInstance.name = nameof(T);
                var newInstanceComponent = newInstance.AddComponent<T>();
                _instance = newInstanceComponent;
            }
        }

        return _instance;
    }

    private static T _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
        }
    }
}
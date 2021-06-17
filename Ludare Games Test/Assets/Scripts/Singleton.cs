using System;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Used for session dependent singletons (Dont' destroy object on Load)
/// </summary>
/// <typeparam name="T"></typeparam>
[DefaultExecutionOrder(-10000)]
public class Singleton<T> : MonoBehaviour where T : Object
{
    public static T Instance => _instance;
    private static T _instance;
    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}
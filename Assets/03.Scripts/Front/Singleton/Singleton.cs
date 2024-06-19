using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _isInitialized = false;
    private static bool _applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                return null;
            }

            if (_instance == null)
            {
                if (!_isInitialized)
                {
                    CreateInstance();
                }
            }
            return _instance;
        }
    }

    private static void CreateInstance()
    {
        lock (_lock)
        {
            if (_instance == null && !_isInitialized && !_applicationIsQuitting)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }

                _isInitialized = true;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
            _isInitialized = false;
        }
    }

    private void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }
}

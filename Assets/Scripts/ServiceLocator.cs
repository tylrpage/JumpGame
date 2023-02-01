using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private Dictionary<Type, object> _services = new();

    public static ServiceLocator Instance { get; private set; }

    private void Awake()
    {
        // Setup singelton
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        
        IService[] foundServices = GetComponents<IService>();
        foreach (IService foundService in foundServices)
        {
            _services.Add(foundService.GetType(), foundService);
        }
    }

    public T GetService<T>() where T : class
    {
        if (_services.TryGetValue(typeof(T), out object obj))
        {
            return obj as T;
        }
        else
        {
            return null;
        }
    }
}

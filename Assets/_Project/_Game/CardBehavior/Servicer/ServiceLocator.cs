using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static Dictionary<Type, IService> services = new Dictionary<Type, IService>();

    public static void Register<T>(T service) where T : class, IService
    {
        services[typeof(T)] = service;
    }

    public static T Get<T>() where T : class, IService
    {
        if (services.TryGetValue(typeof(T), out var service))
        {
            return service as T;
        }

        throw new Exception($"Service of type {typeof(T)} not registered.");
    }
}

public interface IService
{
    
}
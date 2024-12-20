using System;
using System.Collections.Generic;

namespace Core.Service
{
    public class ServiceLocator
    {
        private static Dictionary<Type, IService> services = new Dictionary<Type, IService>();

        public static void Register<T>(T service) where T : IService
        {
            if (services.ContainsKey(typeof(T)))
            {
                services[typeof(T)] = service;
            }
            else
            {
                services.Add(typeof(T), service);
            }
        }

        public static T GetService<T>() where T : IService
        {
            if (services.ContainsKey(typeof(T)))
            {
                return (T)services[typeof(T)];
            }

            throw new KeyNotFoundException($"{typeof(T)} not found!");
        }
    }
}
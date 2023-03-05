using System;
using System.Collections.Generic;

namespace Game.Service
{
    public static class ServiceRegistry
    {
        private static Dictionary<Type, IService> registry;

        public static void Initialize()
        {
            registry = new Dictionary<Type, IService>();
        }

        public static void Bind<T>(T baseService) where T : IService
        {
            registry.Add(typeof(T), baseService);
        }

        public static void UnBind<T>() where T : IService
        {
            registry.Remove(typeof(T));
        }

        public static T Get<T>() where T : IService
        {
            if (registry.TryGetValue(typeof(T), out IService service))
            {
                return (T)service;
            }

            return default(T);
        }
    }

    public interface IService
    {

    }
}

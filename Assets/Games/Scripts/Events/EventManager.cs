using Game.Logger;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Service;

namespace Game.Events
{
    public class EventManager : IService
    {
        public static void Trigger<T>(T eventType) where T : GameEvent
        {
            ServiceRegistry.Get<EventManager>().TriggerEvent<T>(eventType);
        }


        /// <summary>
        /// Registry to save event data
        /// </summary>
        public Dictionary<Type, Action<GameEvent>> eventRegistry;

        public EventManager()
        {
            eventRegistry = new Dictionary<Type, Action<GameEvent>>();
        }

        /// <summary>
        /// Register Event to Registry
        /// </summary>
        /// <typeparam name="T">GameEvent</typeparam>
        /// <param name="listener">Callback function</param>
        public void AddListener<T>(Action<T> listener) where T : GameEvent
        {
            Type type = typeof(T);
            if (eventRegistry.TryGetValue(type, out Action<GameEvent> callback))
            {
                callback += (GameEvent data) => listener?.Invoke((T) data);
                eventRegistry[type] = callback;
            }
            else
            {
                eventRegistry.Add(type, (GameEvent data) => listener?.Invoke((T)data));
            }
        }

        /// <summary>
        /// Remove Event from Registry
        /// </summary>
        /// <typeparam name="T">GameEvent</typeparam>
        /// <param name="listener">Callback function</param>
        public void RemoveListener<T>(Action<T> listener) where T : GameEvent
        {
            Type type = typeof(T);
            if (eventRegistry.TryGetValue(type, out Action<GameEvent> callback))
            {
                callback -= (GameEvent data) => listener?.Invoke((T)data);
                eventRegistry[type] = callback;
            }
        }

        /// <summary>
        /// Remove all Events of type
        /// </summary>
        /// <typeparam name="T">GameEvent</typeparam>
        public void RemoveAllListener<T>() where T : GameEvent
        {
            Type type = typeof(T);
            if (eventRegistry.ContainsKey(type))
            {
                eventRegistry.Remove(type);
            }
        }

        /// <summary>
        /// Trigger Events registered in registry
        /// </summary>
        /// <typeparam name="T">GameEvent</typeparam>
        /// <param name="eventType">Event data</param>
        public void TriggerEvent<T>(T eventType) where T : GameEvent
        {
            Type type = typeof(T);

            if (eventRegistry.TryGetValue(type, out Action<GameEvent> @event))
            {
                Log.Print($"Triggered Event {type.Name}", FilterLog.GameEvent);
                @event?.Invoke(eventType);
            }
        }
    }

    public abstract class GameEvent
    {

    }
}

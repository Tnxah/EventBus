using System;
using System.Collections.Generic;
using System.Linq;

namespace RockInMyShoe.Global.Eventing
{
    public class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> eventListeners = new Dictionary<Type, List<Delegate>>();

        public static void Subscribe<T>(Action<T> listener)
        {
            Type eventType = typeof(T);
            lock (eventListeners)
            {
                if (!eventListeners.ContainsKey(eventType))
                {
                    eventListeners[eventType] = new List<Delegate>();
                }
                eventListeners[eventType].Add(listener);
            }
        }

        public static void Unsubscribe<T>(Action<T> listener)
        {
            Type eventType = typeof(T);
            lock (eventListeners)
            {
                if (eventListeners.ContainsKey(eventType))
                {
                    eventListeners[eventType].Remove(listener);
                }   
            }
            
        }

        public static void Publish<T>(T eventData)
        {
            Type eventType = typeof(T);
            lock (eventListeners)
            {
                if (eventListeners.ContainsKey(eventType))
                {
                    foreach (var listener in eventListeners[eventType].ToList().Cast<Action<T>>())
                    {
                        listener(eventData);
                    }
                }
            }
        }
    }
}

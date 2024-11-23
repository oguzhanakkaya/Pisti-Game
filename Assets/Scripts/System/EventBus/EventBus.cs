using System;
using System.Collections.Generic;

namespace System.EventBus
{
    public class EventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _eventHandlers = new();
    
        public void Subscribe<T>(Action<T> handler)
        {
            var eventType = typeof(T);
            if (!_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] = new List<Delegate>();
            }
            _eventHandlers[eventType].Add(handler);
        }
    
        public void Unsubscribe<T>(Action<T> handler)
        {
            var eventType = typeof(T);
            if (_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType].Remove(handler);
                if (_eventHandlers[eventType].Count == 0)
                {
                    _eventHandlers.Remove(eventType);
                }
            }
        }
    
        public void Fire<T>(T eventData)
        {
            var eventType = typeof(T);
            if (_eventHandlers.ContainsKey(eventType))
            {
                foreach (var handler in _eventHandlers[eventType])
                {
                    ((Action<T>)handler)?.Invoke(eventData);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EventBus
{
    public sealed class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            var eventType = typeof(T);
            if (_handlers.TryGetValue(eventType, out var handlers))
            {
                handlers.Add(handler);
            }
            else
            {
                _handlers.Add(eventType, new List<Delegate> { handler });
            }
        }

        public void Unsubscribe<T>(Action<T> handler) where T : IEvent
        {
            if (!_handlers.TryGetValue(typeof(T), out var handlers)) return;
            var success = handlers.Remove(handler);
            
            if(!success)
                Debug.Log("Couldnt remove handler from eventbus");
        }

        public void Invoke<T>(T item) where T : IEvent
        {
            if (!_handlers.TryGetValue(typeof(T), out var handlers)) return;
            foreach (var handler in handlers)
            {
                handler.DynamicInvoke(item);
            }
        }

        public void CleanUp()
        {
            foreach (var handler in _handlers)
            {
                foreach (var delegateObject in handler.Value.ToList())
                {
                    if (delegateObject.Target.ToString() == "null")
                    {
                        var success = handler.Value.Remove(delegateObject);
                        if (!success)
                            Debug.Log("Couldnt remove handler from eventbus in cleanup");
                    }
                }
            }
        }
    }
}
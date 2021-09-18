using System;
using System.Collections.Generic;

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
            handlers.Remove(handler);
        }

        public void Invoke<T>(T item) where T : IEvent
        {
            if (!_handlers.TryGetValue(typeof(T), out var handlers)) return;
            foreach (var handler in handlers)
            {
                handler.DynamicInvoke(item);
            }
        }
    }
}
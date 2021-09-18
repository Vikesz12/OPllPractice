using System;

namespace EventBus
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> handler) where T : IEvent;
        void Unsubscribe<T>(Action<T> handler) where T : IEvent;
        void Invoke<T>(T item) where T : IEvent;
    }
}

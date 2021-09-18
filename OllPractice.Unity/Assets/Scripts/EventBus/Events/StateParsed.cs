using Model;

namespace EventBus.Events
{
    public class StateParsed : IEvent
    {
        public Face[] Faces { get; set; }
    }
}

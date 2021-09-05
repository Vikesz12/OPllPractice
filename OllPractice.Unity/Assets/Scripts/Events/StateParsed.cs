using Model;

namespace Events
{
    public class StateParsed : IEvent
    {
        public Face[] Faces { get; set; }
    }
}

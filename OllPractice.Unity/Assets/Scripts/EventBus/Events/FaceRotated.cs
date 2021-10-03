using Model;

namespace EventBus.Events
{
    public class FaceRotated : IEvent
    {
        public FaceRotation Rotation { get; set; }
    }
}

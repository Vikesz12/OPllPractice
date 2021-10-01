using Model;
using RubikVisualizers;

namespace EventBus.Events
{
    public class FaceRotated : IEvent
    {
        public FaceRotation Rotation { get; set; }
    }
}

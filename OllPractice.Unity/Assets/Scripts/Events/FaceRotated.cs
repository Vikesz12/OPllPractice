using RubikVisualizers;

namespace Events
{
    public class FaceRotated : IEvent
    {
        public FaceRotation Rotation { get; set; }
    }
}

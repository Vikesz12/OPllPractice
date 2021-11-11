using Model;

namespace EventBus.Events
{
    public class FaceRotated : IEvent
    {
        public FaceRotated(FaceRotation faceRotation) => Rotation = faceRotation;
        public FaceRotation Rotation { get;}
    }
}

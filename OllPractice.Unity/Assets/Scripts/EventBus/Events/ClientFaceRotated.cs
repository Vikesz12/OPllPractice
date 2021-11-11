using Model;

namespace EventBus.Events
{
    public class ClientFaceRotated : IEvent
    {
        public ClientFaceRotated(uint netId, FaceRotation faceRotation)
        {
            NetId = netId;
            FaceRotation = faceRotation;
        }

        public uint NetId { get; }
        public FaceRotation FaceRotation { get; }
    }
}

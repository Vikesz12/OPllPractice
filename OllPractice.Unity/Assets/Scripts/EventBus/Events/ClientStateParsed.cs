using Model;

namespace EventBus.Events
{
    public class ClientStateParsed : IEvent
    {
        public ClientStateParsed(uint netId, Face[] faces)
        {
            NetId = netId;
            Faces = faces;
        }
        public uint NetId { get; }
        public Face[] Faces { get; }
    }
}

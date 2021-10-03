using Model;

namespace EventBus.Events
{
    public class CubeRotated : IEvent
    {
        public CubeRotated( Rotation rotationType, CubeRotation cubeRotation)
        {
            CubeRotation = cubeRotation;
            RotationType = rotationType;
        }

        public CubeRotation CubeRotation { get; }
        public Rotation RotationType { get; }
    }
}

namespace EventBus.Events
{
    public class RotationsEmpty : IEvent
    {
        public RotationsEmpty(float finishTime) => FinishTime = finishTime;

        public float FinishTime { get; }

    }
}

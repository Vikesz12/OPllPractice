namespace EventBus.Events
{
    public class BatteryLevelParsed : IEvent
    {
        public BatteryLevelParsed(float batteryPercent) => BatteryPercent = batteryPercent;

        public float BatteryPercent { get; }
    }
}

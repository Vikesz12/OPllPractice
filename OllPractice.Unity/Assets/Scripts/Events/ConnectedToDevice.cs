namespace Events
{
    public class ConnectedToDevice : IEvent
    {
        public string DeviceId { get; set; }
    }
}

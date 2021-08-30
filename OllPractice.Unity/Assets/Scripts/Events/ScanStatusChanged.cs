namespace Events
{
    public class ScanStatusChanged : IEvent
    {
        public bool Status { get; set; }
    }
}

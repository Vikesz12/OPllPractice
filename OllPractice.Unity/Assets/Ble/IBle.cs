using Parser;
using Scanner;

namespace Ble
{
    public interface IBle
    {
        void StartScan();
        void ScanDevices(RubikScanner rubikScanner);
        void PollData(INotificationParser parser);
        void Subscribe(string deviceId);
        void Write(string data, string deviceId);
        void Quit();
    }
}
using System.Collections.Generic;
using Parser;
using TMPro;

namespace Ble
{
    public interface IBle
    {
        void StartScan(TMP_Dropdown dropdown, List<string> dropdownIds);
        void ScanDevices(TMP_Dropdown dropdown, List<string> dropdownIds, ref string selectedDeviceId, ref bool isScanning);
        void PollData(INotificationParser parser);
        void Subscribe(string deviceId);
        void Write(string data, string deviceId);
        void Quit();
    }
}
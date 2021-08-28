using System;
using System.Collections.Generic;
using Parser;
using TMPro;

namespace Ble
{
    class AndroidBle : IBle
    {
        private Dictionary<string, string> _devices = new Dictionary<string, string>();
        public AndroidBle()
        {
                   }

        public void StartScan(TMP_Dropdown dropdown, List<string> dropdownIds)
        {
                 }

        public void ScanDevices(TMP_Dropdown dropdown, List<string> dropdownIds, ref string selectedDeviceId, ref bool isScanning)
        {
                    }

        public void PollData(INotificationParser parser)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string deviceId)
        {
                    }

        public void Write(string data, string deviceId)
        {
                  }

        public void Quit()
        {
                  }
    }
}

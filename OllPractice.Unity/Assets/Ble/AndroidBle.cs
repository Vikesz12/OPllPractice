using Config;
using Parser;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Ble
{
    class AndroidBle : IBle
    {
        private Dictionary<string, string> _devices = new Dictionary<string, string>();
        private readonly AndroidJavaObject _plugin;

        public AndroidBle()
        {
            _plugin = new AndroidJavaObject("com.viktor.ble.Ble", "RubikScanner", "AndroidMessage");

            _plugin.Call("setSERVICE_UUID", RubikBleConfig.serviceUuid);
            _plugin.Call("setREAD_CHARACTERISTIC_UUID", RubikBleConfig.readCharacteristicUuid);
            _plugin.Call("setWRITE_CHARACTERISTIC_UUID", RubikBleConfig.writeCharacteristicUuid);
            _plugin.Call("setNOTIFY_DESCRIPTOR_UUID", RubikBleConfig.notifyDescriptorUuid);
        }

        public void StartScan(TMP_Dropdown dropdown, List<string> dropdownIds)
        {
            _plugin.Call("scanLeDevice");
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
            _plugin.Call<bool>("connect",deviceId);
        }

        public void Write(string data, string deviceId)
        {
            var bytes = Encoding.ASCII.GetBytes(data);
            var unSignedBytes = Array.ConvertAll(bytes, b => unchecked((sbyte)b));
            _plugin.Call<bool>("write", unSignedBytes);
        }

        public void Quit()
        {
            _plugin.Call("disconnect");
        }
    }
}

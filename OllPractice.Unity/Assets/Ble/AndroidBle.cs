using Config;
using Parser;
using Scanner;
using System;
using System.Text;
using UnityEngine;

namespace Ble
{
    public class AndroidBle : IBle
    {
        private readonly AndroidJavaObject _plugin;

        public AndroidBle()
        {
            _plugin = new AndroidJavaObject("com.viktor.ble.Ble", "AndroidMessageObject", "AndroidMessage");

            _plugin.Call("setSERVICE_UUID", RubikBleConfig.ServiceUuid);
            _plugin.Call("setREAD_CHARACTERISTIC_UUID", RubikBleConfig.ReadCharacteristicUuid);
            _plugin.Call("setWRITE_CHARACTERISTIC_UUID", RubikBleConfig.WriteCharacteristicUuid);
            _plugin.Call("setNOTIFY_DESCRIPTOR_UUID", RubikBleConfig.NotifyDescriptorUuid);
        }

        public void StartScan() => _plugin.Call("scanLeDevice");

        public void ScanDevices(RubikScanner rubikScanner)
        {
        }

        public void PollData(INotificationParser parser) => throw new NotImplementedException();

        public void Subscribe(string deviceId) => _plugin.Call<bool>("connect", deviceId);

        public void Write(string data, string deviceId)
        {
            var bytes = Encoding.ASCII.GetBytes(data);
            var unSignedBytes = Array.ConvertAll(bytes, b => unchecked((sbyte)b));
            _plugin.Call<bool>("write", unSignedBytes);
        }

        public void Quit() => _plugin.Call("disconnect");
    }
}

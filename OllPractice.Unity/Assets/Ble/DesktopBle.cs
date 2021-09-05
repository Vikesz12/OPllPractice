using BleWinrt;
using Config;
using Events;
using Parser;
using Scanner;
using System.Collections.Generic;
using System.Text;

namespace Ble
{
    public class DesktopBle : IBle
    {
        private Dictionary<string, Dictionary<string, string>> _devices = new Dictionary<string, Dictionary<string, string>>();

        public void StartScan() => BleApi.StartDeviceScan();

        public void ScanDevices(RubikScanner rubikScanner)
        {
            var res = new BleApi.DeviceUpdate();
            BleApi.ScanStatus status;
            do
            {
                status = BleApi.PollDevice(ref res, false);
                if (status == BleApi.ScanStatus.AVAILABLE)
                {
                    if (!_devices.ContainsKey(res.id))
                        _devices[res.id] = new Dictionary<string, string>() {
                            { "name", "" },
                            { "isConnectable", "False" }
                        };
                    if (res.nameUpdated)
                        _devices[res.id]["name"] = res.name;
                    if (res.isConnectableUpdated)
                        _devices[res.id]["isConnectable"] = res.isConnectable.ToString();
                    // consider only devices which have a name and which are connectable
                    if (_devices[res.id]["name"] != string.Empty && _devices[res.id]["isConnectable"] == "True")
                    {
                        // add new device to list
                        rubikScanner.CreateNewCubeButton(res.id, _devices[res.id]["name"]);
                    }
                }
                else if (status == BleApi.ScanStatus.FINISHED)
                {
                    EventBus.Instance.Value.Invoke(new ScanStatusChanged { Status = false });
                }
            } while (status == BleApi.ScanStatus.AVAILABLE);
        }

        public void PollData(INotificationParser parser)
        {
            while (BleApi.PollData(out var res, false))
            {
                parser.ParseNotification(res.buf, res.size);
            }
        }

        public void Subscribe(string deviceId)
        {
            if (deviceId == null)
                return;
            BleApi.StopDeviceScan();
            // no error code available in non-blocking mode
            BleApi.SubscribeCharacteristic(deviceId, RubikBleConfig.ServiceUuid, RubikBleConfig.ReadCharacteristicUuid, false);
        }

        public void Write(string dataToWrite, string deviceId)
        {
            var payload = Encoding.ASCII.GetBytes(dataToWrite);
            var data = new BleApi.BLEData
            {
                buf = new byte[512],
                size = (short)payload.Length,
                deviceId = deviceId,
                serviceUuid = RubikBleConfig.ServiceUuid,
                characteristicUuid = RubikBleConfig.WriteCharacteristicUuid
            };
            for (var i = 0; i < payload.Length; i++)
                data.buf[i] = payload[i];
            // no error code available in non-blocking mode
            BleApi.SendData(in data, false);
        }

        public void Quit() => BleApi.Quit();
    }
}
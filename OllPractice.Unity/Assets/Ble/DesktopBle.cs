using System.Collections.Generic;
using System.Linq;
using System.Text;
using BleWinrt;
using Config;
using Parser;
using Scanner;
using TMPro;

namespace Ble
{
    public class DesktopBle : IBle
    {
        private Dictionary<string, Dictionary<string, string>> _devices = new Dictionary<string, Dictionary<string, string>>();

        public void StartScan(TMP_Dropdown dropdown, List<string> dropdownIds)
        {
            BleApi.StartDeviceScan();
        }

        public void ScanDevices(TMP_Dropdown dropdown, List<string> dropdownIds, ref string selectedDeviceId, ref bool isScanning)
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
                        if (dropdownIds.Any(id => id == res.id)) continue;
                        var newDeviceOption = new TMP_Dropdown.OptionData(_devices[res.id]["name"]);
                        dropdownIds.Add(res.id);
                        dropdown.options.Add(newDeviceOption);
                        dropdown.RefreshShownValue();
                        if (dropdownIds.Count == 1) selectedDeviceId = dropdownIds[0];
                    }
                }
                else if (status == BleApi.ScanStatus.FINISHED)
                {
                    isScanning = false;
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
            if(deviceId == null)
                return;
            BleApi.StopDeviceScan();
            // no error code available in non-blocking mode
            BleApi.SubscribeCharacteristic(deviceId, RubikBleConfig.serviceUuid, RubikBleConfig.readCharacteristicUuid, false);
        }

        public void Write(string dataToWrite, string deviceId)
        {
            var payload = Encoding.ASCII.GetBytes(dataToWrite);
            var data = new BleApi.BLEData
            {
                buf = new byte[512],
                size = (short)payload.Length,
                deviceId = deviceId,
                serviceUuid = RubikBleConfig.serviceUuid,
                characteristicUuid = RubikBleConfig.writeCharacteristicUuid
            };
            for (var i = 0; i < payload.Length; i++)
                data.buf[i] = payload[i];
            // no error code available in non-blocking mode
            BleApi.SendData(in data, false);
        }

        public void Quit()
        {
            BleApi.Quit();
        }
    }
}
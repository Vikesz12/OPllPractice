using System;
using System.Collections.Generic;
using System.Linq;
using ArduinoBluetoothAPI;
using Config;
using Parser;
using TMPro;

namespace Ble
{
    class AndroidBle : IBle
    {
        private BluetoothHelper _bluetoothHelper;

        private Dictionary<string, string> _devices = new Dictionary<string, string>();
        public AndroidBle()
        {
            _bluetoothHelper = BluetoothHelper.GetInstance();
            BluetoothHelper.BLE = true;
            _bluetoothHelper.setRxCharacteristic(new BluetoothHelperCharacteristic(RubikBleConfig.readCharacteristicUuid));
            _bluetoothHelper.setTxCharacteristic(new BluetoothHelperCharacteristic(RubikBleConfig.writeCharacteristicUuid));
        }

        public void StartScan(TMP_Dropdown dropdown, List<string> dropdownIds)
        {
            _bluetoothHelper.OnScanEnded += devices =>
            {
                foreach (var bluetoothDevice in devices)
                {
                    if (_devices.ContainsKey(bluetoothDevice.DeviceAddress)) continue;
                    if (dropdownIds.Any(id => id == bluetoothDevice.DeviceAddress)) continue;
                    _devices.Add(bluetoothDevice.DeviceAddress, bluetoothDevice.DeviceName);
                    var newDeviceOption = new TMP_Dropdown.OptionData(bluetoothDevice.DeviceName);
                    dropdownIds.Add(bluetoothDevice.DeviceAddress);
                    dropdown.options.Add(newDeviceOption);
                    dropdown.RefreshShownValue();
                }
            };
        }

        public void ScanDevices(TMP_Dropdown dropdown, List<string> dropdownIds, ref bool isScanning)
        {
            isScanning = _bluetoothHelper.ScanNearbyDevices();
        }

        public void PollData(INotificationParser parser)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string deviceId)
        {
            _bluetoothHelper.setDeviceAddress(deviceId);
            _bluetoothHelper.setDeviceName(_devices[deviceId]);
            _bluetoothHelper.Subscribe(new BluetoothHelperService(RubikBleConfig.serviceUuid));
        }

        public void Write(string data, string deviceId)
        {
            _bluetoothHelper.WriteCharacteristic(new BluetoothHelperCharacteristic(RubikBleConfig.writeCharacteristicUuid), data);
        }

        public void Quit()
        {
            _bluetoothHelper.Disconnect();
        }
    }
}

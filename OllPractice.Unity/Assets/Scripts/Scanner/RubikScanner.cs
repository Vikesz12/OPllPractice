using System;
using System.Collections.Generic;
using System.Text;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scanner
{
    public class RubikScanner : MonoBehaviour
    {
        private bool _isScanningDevices;
        private bool _isSubscribed;
        [SerializeField] private TextMeshProUGUI subscribeText;
        [SerializeField] private TMP_Dropdown _dropdown;
        private Dictionary<string, Dictionary<string, string>> _devices = new Dictionary<string, Dictionary<string, string>>();
        private string _selectedDeviceId;

        private void Start()
        {
            _dropdown.ClearOptions();
            _dropdown.onValueChanged.AddListener(OnDropDownSelected);
        }

        private void OnDropDownSelected(int arg0)
        {
            _selectedDeviceId = _dropdown.options[arg0].text;
        }

        public void StartScan()
        {
            _dropdown.ClearOptions();
            BleApi.StartDeviceScan();
            _isScanningDevices = true;
            Debug.Log($"Scanning {_isScanningDevices}");
        }

        public void Update()
        {
            if (_isScanningDevices)
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
                        if (_devices[res.id]["name"] != "" && _devices[res.id]["isConnectable"] == "True")
                        {
                            // add new device to list
                            var newDeviceOption = new TMP_Dropdown.OptionData(_devices[res.id]["name"]);
                            _dropdown.options.Add(newDeviceOption);
                        }
                    }
                    else if (status == BleApi.ScanStatus.FINISHED)
                    {
                        _isScanningDevices = false;
                    }
                } while (status == BleApi.ScanStatus.AVAILABLE);
            }
            if (_isSubscribed)
            {
                while (BleApi.PollData(out var res, false))
                {
                    subscribeText.text = BitConverter.ToString(res.buf, 0, res.size);
                    // subscribeText.text = Encoding.ASCII.GetString(res.buf, 0, res.size);
                }
            }
        }
        public void Subscribe()
        {
            // no error code available in non-blocking mode
            BleApi.SubscribeCharacteristic(_selectedDeviceId, RubikBleConfig.serviceUuid, RubikBleConfig.readCharacteristicUuid, false);
            _isSubscribed = true;
        }

        public void Write()
        {
            var payload = Encoding.ASCII.GetBytes("3");
            var data = new BleApi.BLEData
            {
                buf = new byte[512],
                size = (short) payload.Length,
                deviceId = _selectedDeviceId,
                serviceUuid = RubikBleConfig.serviceUuid,
                characteristicUuid = RubikBleConfig.writeCharacteristicUuid
            };
            for (var i = 0; i < payload.Length; i++)
                data.buf[i] = payload[i];
            // no error code available in non-blocking mode
            BleApi.SendData(in data, false);
        }
        private void OnApplicationQuit()
        {
            BleApi.Quit();
        }

        private void OnDestroy()
        {
            _dropdown.onValueChanged.RemoveAllListeners();
        }
    }
}

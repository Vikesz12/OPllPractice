using Config;
using Injecter;
using Parser;
using RubikVisualizers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace Scanner
{
    public class RubikScanner : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private RubikHolder _rubikHolder;

        [Inject] private INotificationParser _notificationParser;

        private bool _isScanningDevices;
        private bool _isSubscribed;
        private List<string> _dropdownIds;
        private Dictionary<string, Dictionary<string, string>> _devices = new Dictionary<string, Dictionary<string, string>>();
        private string _selectedDeviceId;

        private void Start()
        {
            _dropdown.ClearOptions();
            _dropdownIds = new List<string>();
            _dropdown.onValueChanged.AddListener(OnDropDownSelected);
        }

        private void OnDropDownSelected(int arg0)
        {
            _selectedDeviceId = _dropdownIds[arg0];
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
                        if (_devices[res.id]["name"] != string.Empty && _devices[res.id]["isConnectable"] == "True")
                        {
                            // add new device to list
                            if (_dropdownIds.Any(id => id == res.id)) continue;
                            var newDeviceOption = new TMP_Dropdown.OptionData(_devices[res.id]["name"]);
                            _dropdownIds.Add(res.id);
                            _dropdown.options.Add(newDeviceOption);
                            _dropdown.RefreshShownValue();
                            if (_dropdownIds.Count == 1) _selectedDeviceId = _dropdownIds[0];
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
                    _notificationParser.ParseNotification(res.buf, res.size);
                }
            }
        }
        public void Subscribe()
        {
            // no error code available in non-blocking mode
            BleApi.SubscribeCharacteristic(_selectedDeviceId, RubikBleConfig.serviceUuid, RubikBleConfig.readCharacteristicUuid, false);
            _isSubscribed = true;
            Debug.Log($"connected to {_selectedDeviceId}");
        }

        public void Write(string writeData)
        {
            var payload = Encoding.ASCII.GetBytes(writeData);
            var data = new BleApi.BLEData
            {
                buf = new byte[512],
                size = (short)payload.Length,
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

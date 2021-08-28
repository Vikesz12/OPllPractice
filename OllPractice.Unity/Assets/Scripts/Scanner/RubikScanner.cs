using Ble;
using Parser;
using RubikVisualizers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scanner
{
    public class RubikScanner : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private RubikHolder _rubikHolder;

        [Inject] private INotificationParser _notificationParser;
        [Inject] private IBle _bleScanner;

        private bool _isScanningDevices;
        private bool _isSubscribed;
        private List<string> _dropdownIds;
        private string _selectedDeviceId;

        private void Start()
        {
            _dropdown.ClearOptions();
            _dropdownIds = new List<string>();
            _dropdown.onValueChanged.AddListener(OnDropDownSelected);
        }

        private void OnDropDownSelected(int arg0) => _selectedDeviceId = _dropdownIds[arg0];

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
                _bleScanner.ScanDevices(_dropdown, _dropdownIds, ref _isScanningDevices);
            }
            if (_isSubscribed)
            {
                _bleScanner.PollData(_notificationParser);
            }
        }
        public void Subscribe()
        {
            _bleScanner.Subscribe(_selectedDeviceId);
            _isSubscribed = true;
            Debug.Log($"connected to {_selectedDeviceId}");
        }

        public void Write(string writeData) => _bleScanner.Write(writeData, _selectedDeviceId);
        private void OnApplicationQuit() => _bleScanner.Quit();
        private void OnDestroy() => _dropdown.onValueChanged.RemoveAllListeners();
    }
}

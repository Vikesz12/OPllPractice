﻿using System;
using Ble;
using Parser;
using RubikVisualizers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BleWinrt;
using TMPro;
using UnityEngine;
using Zenject;

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
            _bleScanner.StartScan(_dropdown, _dropdownIds);
            _isScanningDevices = true;
            Debug.Log($"Scanning {_isScanningDevices}");
        }

        public void Update()
        {
            if (_isScanningDevices)
            {
                _bleScanner.ScanDevices(_dropdown, _dropdownIds, ref _selectedDeviceId, ref _isScanningDevices);
            }
            if (_isSubscribed)
            {
#if !UNITY_ANDROID
                _bleScanner.PollData(_notificationParser);
#endif
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

        public void AndroidMessage(string message)
        {
            var messageParts = message.Split('|');
            if (messageParts[0] == "notification")
            {
                var bytes = Convert.FromBase64String(messageParts[1]);
                _notificationParser.ParseNotification(bytes, (short)bytes.Length);
            }
            else
            {
                if (_dropdownIds.Any(id => id == messageParts[1])) return;
                var newDeviceOption = new TMP_Dropdown.OptionData(messageParts[2]);
                _dropdownIds.Add(messageParts[1]);
                _dropdown.options.Add(newDeviceOption);
                _dropdown.RefreshShownValue();
                if (_dropdownIds.Count == 1) _selectedDeviceId = _dropdownIds[0]; 
            }
        }
    }
}

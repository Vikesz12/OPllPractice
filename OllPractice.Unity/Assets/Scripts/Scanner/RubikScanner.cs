using System;
using System.Collections;
using Ble;
using Parser;
using RubikVisualizers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BleWinrt;
using Events;
using TMPro;
using UnityEngine;
using Zenject;

namespace Scanner
{
    public class RubikScanner : MonoBehaviour
    {
        [SerializeField] private Transform _foundCubeButtonsParent;

        [Inject] private INotificationParser _notificationParser;
        [Inject] private IBle _bleScanner;

        private bool _isScanningDevices;
        private bool _isSubscribed;
        private string _connectedDeviceId;
        private List<string> _foundIds;

        private void Start()
        {
            EventBus.Instance.Value.Subscribe<ScanStatusChanged>(ScanStatusChanged);
            _foundIds = new List<string>();
            StartScan();
        }

        private void ScanStatusChanged(ScanStatusChanged obj) => _isScanningDevices = obj.Status;

        public void StartScan()
        {
            _bleScanner.StartScan();
            EventBus.Instance.Value.Invoke(new ScanStatusChanged { Status = true });
            Debug.Log($"Scanning {_isScanningDevices}");
#if UNITY_ANDROID
            StartCoroutine(ScanFinished());
#endif
        }

        public void Update()
        {
            if (_isScanningDevices)
            {
                _bleScanner.ScanDevices(this);
            }
            if (_isSubscribed)
            {
#if !UNITY_ANDROID
                _bleScanner.PollData(_notificationParser);
#endif
            }
        }

        private void ConnectToCube(string deviceId)
        {
            _bleScanner.Subscribe(deviceId);
            _isSubscribed = true;
            Debug.Log($"connected to {deviceId}");
            _connectedDeviceId = deviceId;
            EventBus.Instance.Value.Invoke(new ConnectedToDevice{DeviceId = deviceId});
        }

        public void Write(string writeData) => _bleScanner.Write(writeData, _connectedDeviceId);
        private void OnApplicationQuit() => _bleScanner.Quit();

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
                CreateNewCubeButton(messageParts[1],messageParts[2]);
            }
        }

        public void CreateNewCubeButton(string deviceId, string cubeName)
        {
            if (_foundIds.Any(id => id == deviceId)) return;
            _foundIds.Add(deviceId);
            var newCubeButton = Instantiate(Resources.Load<GameObject>("Prefabs/FoundCubeButton"),_foundCubeButtonsParent);
            var foundCubeButton = newCubeButton.GetComponent<FoundCubeButton>();
            foundCubeButton.SetCubeName(cubeName);
            foundCubeButton.GetButton.onClick.AddListener(() => ConnectToCube(deviceId));
        }

        private static IEnumerator ScanFinished()
        {
            yield return new WaitForSeconds(15);
            EventBus.Instance.Value.Invoke(new ScanStatusChanged{Status = false});
        }
    }
}

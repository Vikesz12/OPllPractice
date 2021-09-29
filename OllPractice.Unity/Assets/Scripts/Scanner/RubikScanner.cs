using Ble;
using EventBus;
using EventBus.Events;
using Parser;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Scanner
{
    public class RubikScanner : MonoBehaviour
    {
        [SerializeField] private Transform _foundCubeButtonsParent;

        [Inject] private INotificationParser _notificationParser;
        [Inject] private IBle _bleScanner;
        [Inject] private IEventBus _eventBus;

        private bool _isScanningDevices;
        private bool _isSubscribed;
        private string _connectedDeviceId;
        private List<string> _foundIds;

        private void Awake()
        {
            _eventBus.Subscribe<ScanStatusChanged>(ScanStatusChanged);
            _foundIds = new List<string>();
            if (ConnectedDeviceData.ConnectedDeviceId != null)
            {
                _isSubscribed = true;
                _connectedDeviceId = ConnectedDeviceData.ConnectedDeviceId;
            }
            else
            {
                StartScan();
            }
        }

        private void OnDestroy() => _eventBus.Unsubscribe<ScanStatusChanged>(ScanStatusChanged);

        private void ScanStatusChanged(ScanStatusChanged obj) => _isScanningDevices = obj.Status;

        public void StartScan()
        {
            _bleScanner.StartScan();
            _eventBus.Invoke(new ScanStatusChanged { Status = true });
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
            ConnectedDeviceData.ConnectedDeviceId = deviceId;
            _eventBus.Invoke(new ConnectedToDevice { DeviceId = deviceId });
        }

        public void Write(string writeData) => _bleScanner.Write(writeData, _connectedDeviceId);
        private void OnApplicationQuit() => _bleScanner.Quit();

        public void CreateNewCubeButton(string deviceId, string cubeName)
        {
            if (_foundIds.Any(id => id == deviceId)) return;
            _foundIds.Add(deviceId);
            var newCubeButton = Instantiate(Resources.Load<GameObject>("Prefabs/FoundCubeButton"), _foundCubeButtonsParent);
            var foundCubeButton = newCubeButton.GetComponent<FoundCubeButton>();
            foundCubeButton.SetCubeName(cubeName);
            foundCubeButton.GetButton.onClick.AddListener(() => ConnectToCube(deviceId));
        }

        // ReSharper disable once UnusedMember.Local
        private IEnumerator ScanFinished()
        {
            yield return new WaitForSeconds(15);
            _eventBus.Invoke(new ScanStatusChanged { Status = false });
        }
    }
}

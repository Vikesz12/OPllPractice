using Ble;
using Cysharp.Threading.Tasks;
using EventBus;
using EventBus.Events;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace RotationVisualizer
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LoadCubeBatteryLevel : MonoBehaviour
    {
        [Inject] private readonly IBle _ble;
        [Inject] private readonly IEventBus _eventBus;

        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _eventBus.Subscribe<ConnectedToDevice>(device => OnConnect(device).Forget());
            _eventBus.Subscribe<BatteryLevelParsed>(BatteryParsed);
            gameObject.SetActive(false);
        }

        private async UniTask OnConnect(ConnectedToDevice obj)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            _ble.Write("2", ConnectedDeviceData.ConnectedDeviceId);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<BatteryLevelParsed>(BatteryParsed);
            _eventBus.Unsubscribe<ConnectedToDevice>(device => OnConnect(device).Forget());
        }

        private void BatteryParsed(BatteryLevelParsed batteryLevel)
        {
            gameObject.SetActive(true);
            _text.text = batteryLevel.BatteryPercent*100f + "%";
        }
    }
}
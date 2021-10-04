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
            _eventBus.Subscribe<BatteryLevelParsed>(BatteryParsed);
            _text.text = string.Empty; 
            OnConnect().Forget();
        }

        private async UniTask OnConnect()
        {
            while (ConnectedDeviceData.ConnectedDeviceId == null)
            {
                await UniTask.Yield();
            }

            await UniTask.Delay(TimeSpan.FromSeconds(2));
            _ble.Write("2", ConnectedDeviceData.ConnectedDeviceId);
        }

        private void OnDestroy() => _eventBus.Unsubscribe<BatteryLevelParsed>(BatteryParsed);

        private void BatteryParsed(BatteryLevelParsed batteryLevel) => _text.text = batteryLevel.BatteryPercent * 100f + "%";
    }
}
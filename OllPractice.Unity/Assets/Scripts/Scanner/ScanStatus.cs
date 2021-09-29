using System;
using EventBus;
using EventBus.Events;
using TMPro;
using UnityEngine;
using Zenject;

namespace Scanner
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScanStatus : MonoBehaviour
    {
        [Inject] private IEventBus _eventBus;

        private TextMeshProUGUI _scanText;

        private void Start()
        {
            _scanText = GetComponent<TextMeshProUGUI>();
            _eventBus.Subscribe<ScanStatusChanged>(StatusChanged);
        }

        private void OnDestroy() => _eventBus.Unsubscribe<ScanStatusChanged>(StatusChanged);

        private void StatusChanged(ScanStatusChanged statusChanged)
            => _scanText.text = statusChanged.Status ? "Scanning..." : "Scan finished";
    }
}

using Events;
using TMPro;
using UnityEngine;

namespace Scanner
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScanStatus : MonoBehaviour
    {
        private TextMeshProUGUI _scanText;

        private void Start()
        {
            _scanText = GetComponent<TextMeshProUGUI>();
            EventBus.Instance.Value.Subscribe<ScanStatusChanged>(StatusChanged);
        }

        private void StatusChanged(ScanStatusChanged statusChanged) 
            => _scanText.text = statusChanged.Status ? "Scanning..." : "Scan finished";
    }
}

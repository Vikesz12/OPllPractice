using System;
using TMPro;
using UnityEngine;

namespace Timers
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class RubikTimer : MonoBehaviour
    {
        private TextMeshProUGUI _timerText;
        private bool _isRunning;

        private void Start() => _timerText = GetComponent<TextMeshProUGUI>();
        private void Update()
        {
            if (!_isRunning) return;

            TimeElapsed += Time.deltaTime;
            var span = TimeSpan.FromSeconds(TimeElapsed);
            _timerText.text = TimeElapsed > 60f ? $@"{span:m\:ss\.ff}" : $@"{span:ss\.ff}";
        }

        public float TimeElapsed { get; private set; }

        public void StartTimer()
        {
            _isRunning = true;
            TimeElapsed = 0f;
        }

        public void StopTimer() => _isRunning = false;

        
    }
}

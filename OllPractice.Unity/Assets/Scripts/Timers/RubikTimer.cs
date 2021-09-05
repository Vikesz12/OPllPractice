using System;
using System.Timers;
using TMPro;
using UnityEngine;

namespace Timers
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class RubikTimer : MonoBehaviour
    {
        private TextMeshProUGUI _timerText;
        private bool _isRunning;
        private float _elapsed;
        public void Start() => _timerText = GetComponent<TextMeshProUGUI>();

        public void StartTimer()
        {
            _isRunning = true;
            _elapsed = 0f;
        }

        public void StopTimer() => _isRunning = false;

        private void Update()
        {
            if(!_isRunning) return;

            _elapsed += Time.deltaTime;
            var span = TimeSpan.FromSeconds(_elapsed);
            if (_elapsed > 60f)
            {
                _timerText.text = $@"{span:m\:ss\.ff}";
            }
            else
            {
                _timerText.text = $@"{span:ss\.ff}";
            }
        }
    }
}

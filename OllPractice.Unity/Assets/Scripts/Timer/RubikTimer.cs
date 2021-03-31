using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Timer
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class RubikTimer : MonoBehaviour
    {
        private TextMeshProUGUI _timerText;
        private bool _running;
        private float _minutes;
        private float _seconds;
        private float _milliseconds;

        public void Start() => _timerText = GetComponent<TextMeshProUGUI>();

        public void StartTimer()
        {
            _running = true;
            _minutes = 0;
            _seconds = 0;
            _milliseconds = 0;
        }

        public void StopTimer() => _running = false;

        private string GetMinutesText()
        {
            return _minutes == 0 ? string.Empty : _minutes.ToString(CultureInfo.InvariantCulture) + ":";
        }
        public void Update()
        {
            if(!_running) return;

            if (_milliseconds >= 100)
            {
                _seconds++;
                if (_seconds >= 60)
                {
                    _minutes++;
                    _seconds -= 60;
                }

                _milliseconds -= 100;
            }

            _milliseconds += Time.deltaTime * 100;


            _timerText.text = $"{GetMinutesText()}{_seconds}:{(int) _milliseconds}";
        }
    }
}

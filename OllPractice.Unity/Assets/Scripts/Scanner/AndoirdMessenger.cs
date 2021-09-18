using System;
using EventBus;
using Parser;
using UnityEngine;
using Zenject;

namespace Scanner
{
    public class AndoirdMessenger : MonoBehaviour
    {
        [Inject] private INotificationParser _notificationParser;
        [Inject] private IEventBus _eventBus;

        [SerializeField] private RubikScanner _rubikScanner;

        private void Awake() => _eventBus.CleanUp();

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
                if(_rubikScanner == null) return;

                _rubikScanner.CreateNewCubeButton(messageParts[1], messageParts[2]);
            }
        }
    }
}

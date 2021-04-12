using Model;
using Parser;
using UnityEngine;

namespace RubikVisualizers
{
    public class RubikHolder : MonoBehaviour
    {
        private RubikVisualizer _currentVisualizer;
        private NotificationParser _notificationParser;
        private bool _flipped;

        public void Awake() => CreateVisualizer();

        private void CreateVisualizer()
        {
            _currentVisualizer = Instantiate(Resources.Load<GameObject>("Prefabs/RubiksConnected"), transform).GetComponent<RubikVisualizer>();
        }

        public void AddNotificationParser(NotificationParser notificationParser)
        {
            _notificationParser = notificationParser;
            RegisterToParserEvents();
        }

        private void RegisterToParserEvents()
        {
            _notificationParser.FaceRotated += _currentVisualizer.NotificationParserOnFaceRotated;
            _notificationParser.StateParsed += _currentVisualizer.LoadState;
        }
        private void UnRegisterEvents()
        {
            _notificationParser.FaceRotated -= _currentVisualizer.NotificationParserOnFaceRotated;
            _notificationParser.StateParsed -= _currentVisualizer.LoadState;
        }

        public void LoadState(Face[] state)
        {
            ResetVisualizer();
            _currentVisualizer.LoadState(state);
        }

        public void Flip()
        {
            if(_flipped) return;
            _flipped = true;
            _currentVisualizer.Flip();
        }

        public void ResetVisualizer()
        {
            UnRegisterEvents();
            Destroy(_currentVisualizer.gameObject);
            _flipped = false;
            CreateVisualizer();
            RegisterToParserEvents();
        }
    }
}

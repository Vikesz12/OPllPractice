using RubikVisualizers;
using System.Collections.Generic;
using Parser;
using TMPro;
using UnityEngine;

namespace RotationVisualizer
{
    public class RotationMessenger : MonoBehaviour
    {

        [SerializeField] private Transform _messagesParent;
        [SerializeField] private Transform _wrongMessageParent;

        private List<FaceRotation> _rotations;
        private GameObject _rotationMessagePrefab;
        private int _currentPosition;
        private const int MAXMessageCount = 10;

        public void Start()
        {
            _rotationMessagePrefab = Resources.Load<GameObject>("Prefabs/RotationMessage");
            LoadRotations(new[] { FaceRotation.R, FaceRotation.U, FaceRotation.RPrime, FaceRotation.UPrime });
        }

        public void RegisterNotificationParser(NotificationParser notificationParser)
        {
            notificationParser.FaceRotated += NotificationParserOnFaceRotated;
        }

        private void NotificationParserOnFaceRotated(FaceRotation rotation)
        {
            if(_rotations.Count == 0) return;
            var messageObject = _messagesParent.GetChild(_currentPosition % MAXMessageCount);
            var textComponent = messageObject.GetComponent<TextMeshProUGUI>();

            textComponent.color = rotation == _rotations[_currentPosition] ? Color.green : Color.red;
            _currentPosition += 1;
            if (_currentPosition > _rotations.Count-1)
                Clear();
            else if(_currentPosition % MAXMessageCount == 0)
                ShowNextBatch();
        }

        private void Clear()
        {
            DeleteMessageObjects();
            _rotations.Clear();
        }

        private void DeleteMessageObjects()
        {
            foreach (Transform child in _messagesParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void LoadRotations(IEnumerable<FaceRotation> rotations)
        {
            _currentPosition = 0;
            _rotations = new List<FaceRotation>(rotations);
            ShowNextBatch();
        }

        private void ShowNextBatch()
        {
            DeleteMessageObjects();
            var endPosition = _currentPosition +  MAXMessageCount> _rotations.Count ? _rotations.Count : _currentPosition + MAXMessageCount;
            for (var i = _currentPosition; i < endPosition; i++)
            {
                var createdMessage = Instantiate(_rotationMessagePrefab, _messagesParent);
                createdMessage.GetComponent<TextMeshProUGUI>().text = _rotations[i].ToRubikNotation();
            }
        }
    }
}

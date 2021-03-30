using Parser;
using RubikVisualizers;
using System;
using System.Collections.Generic;
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
        private Stack<FaceRotation> _correctionTurns;

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
            if (_rotations.Count == 0 && _correctionTurns.Count == 0) return;
            if (_correctionTurns.Count != 0)
            {
                if (rotation == _correctionTurns.Peek())
                {
                    _correctionTurns.Pop();
                    Destroy(_wrongMessageParent.GetChild(0).gameObject);
                    if (_correctionTurns.Count == 0)
                    {
                        _messagesParent.GetChild(_currentPosition % MAXMessageCount).GetComponent<TextMeshProUGUI>().color = Color.black;
                    }
                }
                else
                {
                    AddCorrectionTurnFor(rotation);
                }
            }
            else
            {
                var messageObject = _messagesParent.GetChild(_currentPosition % MAXMessageCount);
                var textComponent = messageObject.GetComponent<TextMeshProUGUI>();

                if (rotation == _rotations[_currentPosition])
                {
                    textComponent.color = Color.green;
                    _currentPosition += 1;
                }
                else
                {
                    textComponent.color = Color.red;
                    AddCorrectionTurnFor(rotation);
                }

                if (_currentPosition > _rotations.Count - 1)
                    Clear();
                else if (_currentPosition % MAXMessageCount == 0)
                    ShowNextBatch();
            }
        }

        private void AddCorrectionTurnFor(FaceRotation rotation)
        {
            var correctionTurn = GetInvertedTurn(rotation);
            _correctionTurns.Push(correctionTurn);
            var createdMessage = Instantiate(_rotationMessagePrefab, _wrongMessageParent);
            createdMessage.GetComponent<TextMeshProUGUI>().text = correctionTurn.ToRubikNotation();
            createdMessage.transform.SetAsFirstSibling();
        }

        private static FaceRotation GetInvertedTurn(FaceRotation rotation)
        {
            if (rotation.ToString().Contains("Prime"))
            {
                Enum.TryParse(rotation.ToString().Replace("Prime", ""), out FaceRotation result);
                return result;
            }
            else
            {
                Enum.TryParse(rotation + "Prime", out FaceRotation result);
                return result;
            }
        }

        private void Clear()
        {
            DeleteMessageObjects();
            _rotations.Clear();
            _correctionTurns.Clear();
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
            _correctionTurns = new Stack<FaceRotation>();
            ShowNextBatch();
        }

        private void ShowNextBatch()
        {
            DeleteMessageObjects();
            var endPosition = _currentPosition + MAXMessageCount > _rotations.Count ? _rotations.Count : _currentPosition + MAXMessageCount;
            for (var i = _currentPosition; i < endPosition; i++)
            {
                var createdMessage = Instantiate(_rotationMessagePrefab, _messagesParent);
                createdMessage.GetComponent<TextMeshProUGUI>().text = _rotations[i].ToRubikNotation();
            }
        }
    }
}

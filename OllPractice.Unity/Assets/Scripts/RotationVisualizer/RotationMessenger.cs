using Parser;
using RubikVisualizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventBus;
using EventBus.Events;
using Timers;
using TMPro;
using UnityEngine;
using Zenject;

namespace RotationVisualizer
{
    public class RotationMessenger : MonoBehaviour
    {
        [SerializeField] private Transform _messagesParent;
        [SerializeField] private Transform _wrongMessageParent;
        [SerializeField] private RubikTimer _rubikTimer;
        [SerializeField] private RubikHolder _rubikHolder;

        [Inject] private INotificationParser _notificationParser;
        [Inject] private IEventBus _eventBus;

        private List<FaceRotation> _rotations;
        private GameObject _rotationMessagePrefab;
        private int _currentPosition;
        private const int MAXMessageCount = 10;
        private Stack<FaceRotation> _correctionTurns;
        private bool _f2LMode;
        private int _yTurns;
        public Action PracticeFinished;
        private bool _animating;

        public void Start()
        {
            _rotationMessagePrefab = Resources.Load<GameObject>("Prefabs/RotationMessage");
            _rotations = new List<FaceRotation>();
            _correctionTurns = new Stack<FaceRotation>();
            _eventBus.Subscribe<FaceRotated>(NotificationParserOnFaceRotated);
        }

        private void OnDestroy() => _eventBus.Unsubscribe<FaceRotated>(NotificationParserOnFaceRotated);

        public async void AnimateCurrentMoves()
        {
            _animating = true;
            var remainingRotations = _rotations
                .GetRange(_currentPosition, _rotations.Count)
                .Select(r => r.ToF2LRotation(_yTurns))
                .Where(r => r != FaceRotation.Y && r != FaceRotation.YPrime);
            await _notificationParser.AnimateRotations(remainingRotations).ConfigureAwait(false);
        }

        private void NotificationParserOnFaceRotated(FaceRotated faceRotated)
        {
            var rotation = faceRotated.Rotation;
            if (_rotations.Count == 0 && _correctionTurns.Count == 0) return;
            if (_correctionTurns.Count != 0)
            {
                if (CheckCorrectTurn(rotation, _correctionTurns.Peek()))
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

                if (_rotations[_currentPosition] == FaceRotation.Y || _rotations[_currentPosition] == FaceRotation.YPrime)
                {
                    if (_rotations[_currentPosition] == FaceRotation.Y)
                    {
                        _rubikHolder.GetCurrentVisualizer().Y();
                        _yTurns++;
                    }
                    else
                    {
                        _rubikHolder.GetCurrentVisualizer().YPrime();
                        _yTurns--;
                    }

                    _currentPosition++;
                    textComponent.color = Color.blue;
                    textComponent = _messagesParent.GetChild(_currentPosition % MAXMessageCount).GetComponent<TextMeshProUGUI>();
                }

                if (CheckCorrectTurn(rotation, _rotations[_currentPosition]))
                {
                    if (_currentPosition == 0)
                        _rubikTimer.StartTimer();

                    textComponent.color = Color.green;
                    _currentPosition++;

                    if (_currentPosition == _rotations.Count)
                        _rubikTimer.StopTimer();
                }
                else
                {
                    textComponent.color = Color.red;
                    AddCorrectionTurnFor(rotation);
                }

                if (_currentPosition > _rotations.Count - 1)
                {
                    if (!_f2LMode)
                    {
                        Clear();
                    }
                    else
                    {
                        _currentPosition = 0;
                        ShowNextBatch();
                        _yTurns = 0;
                        if(_animating)
                        {
                            StartCoroutine(PracticeFinishedCoroutine());
                        }
                        else
                        {
                            StartCoroutine(PracticeFinishedCoroutine());
                        }
                    }

                }
                else if (_currentPosition % MAXMessageCount == 0)
                {
                    ShowNextBatch();
                }
            }
        }

        private IEnumerator PracticeFinishedCoroutine()
        {
            yield return new WaitForSeconds(3);
            _animating = false;
            PracticeFinished?.Invoke();
        }

        private bool CheckCorrectTurn(FaceRotation rotationToCheck, FaceRotation correctFaceRotation)
        {
            if (!_f2LMode)
                return rotationToCheck == correctFaceRotation;
            return rotationToCheck == correctFaceRotation.ToF2LRotation(_yTurns);
        }
        private void AddCorrectionTurnFor(FaceRotation rotation)
        {
            var correctionTurn = GetInvertedTurn(_f2LMode ? rotation.ToF2LRotation(_yTurns) : rotation);

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

        public void LoadRotations(IEnumerable<FaceRotation> rotations, bool f2LMode)
        {
            _currentPosition = 0;
            _f2LMode = f2LMode;
            _rotations = rotations.ToList();
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

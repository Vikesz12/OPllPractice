using EventBus;
using EventBus.Events;
using Model;
using Parser;
using RubikVisualizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [Inject] private readonly INotificationParser _notificationParser;
        [Inject] private readonly IEventBus _eventBus;

        private List<FaceRotation> _rotations;
        private GameObject _rotationMessagePrefab;
        private int _currentPosition;
        private const int MAXMessageCount = 10;
        private Stack<FaceRotation> _correctionTurns;
        private readonly List<FaceRotation> _cubeRotations = new List<FaceRotation>();
        private bool _animating;
        private bool _practiceMode;

        public void Awake()
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
                .Select(r => r.ToCubeTurnedRotation(_cubeRotations))
                .Where(r => !r.IsCubeRotation);
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

                if (_rotations[_currentPosition].IsCubeRotation)
                {
                    var rotationType = _rotations[_currentPosition].RotationType;
                    switch (_rotations[_currentPosition].CubeRotation)
                    {
                        case CubeRotation.Y:
                            if (rotationType == Rotation.One)
                            {
                                _rubikHolder.GetCurrentVisualizer().Y();
                                _cubeRotations.Add(new FaceRotation(CubeRotation.Y, Rotation.One));
                            }
                            else
                            {
                                _rubikHolder.GetCurrentVisualizer().YPrime();
                                _cubeRotations.Add(new FaceRotation(CubeRotation.Y, Rotation.Prime));
                            }
                            break;
                        case CubeRotation.X:
                            if (rotationType == Rotation.One)
                            {
                                _rubikHolder.GetCurrentVisualizer().X();
                                _cubeRotations.Add(new FaceRotation(CubeRotation.X, Rotation.One));
                            }
                            else
                            {
                                _rubikHolder.GetCurrentVisualizer().XPrime();
                                _cubeRotations.Add(new FaceRotation(CubeRotation.X, Rotation.Prime));
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }


                    _currentPosition++;

                    if (_currentPosition == 0)
                        _rubikTimer.StartTimer();
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
                    if (!_practiceMode)
                    {
                        Clear();
                    }
                    else
                    {
                        _currentPosition = 0;
                        StartCoroutine(PracticeFinishedCoroutine());
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
            _eventBus.Invoke(new RotationsEmpty(_rubikTimer.TimeElapsed));
        }

        private bool CheckCorrectTurn(FaceRotation rotationToCheck, FaceRotation correctFaceRotation) 
            => Equals(rotationToCheck, correctFaceRotation.ToCubeTurnedRotation(_cubeRotations));

        private void AddCorrectionTurnFor(FaceRotation rotation)
        {
            var correctionTurn = GetInvertedTurn(rotation.ToCubeTurnedRotation(_cubeRotations));

            _correctionTurns.Push(correctionTurn);
            var createdMessage = Instantiate(_rotationMessagePrefab, _wrongMessageParent);
            createdMessage.GetComponent<TextMeshProUGUI>().text = correctionTurn.ToRubikNotation();
            createdMessage.transform.SetAsFirstSibling();
        }

        private static FaceRotation GetInvertedTurn(FaceRotation rotation) =>
            new FaceRotation(rotation.BasicRotation,
                rotation.RotationType == Rotation.One ? Rotation.Prime : Rotation.One);

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
            if (f2LMode)
            {
                _cubeRotations.Add(new FaceRotation(CubeRotation.X, Rotation.One));
                _cubeRotations.Add(new FaceRotation(CubeRotation.X, Rotation.One));
                _cubeRotations.Add(new FaceRotation(CubeRotation.Y, Rotation.One));
                _practiceMode = true;
            }
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

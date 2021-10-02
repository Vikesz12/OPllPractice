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
        private readonly List<RotationStep> _rotationSteps = new List<RotationStep>();
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
            _rubikTimer.StartTimer();
            if (_rotations.Count == 0 && _correctionTurns.Count == 0) return;
            if (_correctionTurns.Count != 0)
            {
                if (CheckCorrectTurn( _correctionTurns.Peek(), rotation))
                {
                    _correctionTurns.Pop();
                    Destroy(_wrongMessageParent.GetChild(0).gameObject);
                    if (_correctionTurns.Count == 0)
                    {
                        _messagesParent.GetChild(_currentPosition % MAXMessageCount).GetComponent<RotationStep>().ResetColor();
                    }
                }
                else
                {
                    AddCorrectionTurnFor(rotation);
                }
            }
            else
            {
                var rotationStep = _rotationSteps[_currentPosition % MAXMessageCount];

                if (_rotations[_currentPosition].IsCubeRotation)
                {
                    var rotationType = _rotations[_currentPosition].RotationType;
                    switch (_rotations[_currentPosition].CubeRotation)
                    {
                        case CubeRotation.y:
                            if (rotationType == Rotation.One)
                            {
                                _rubikHolder.GetCurrentVisualizer().Y();
                                _cubeRotations.Add(new FaceRotation(CubeRotation.y, Rotation.One));
                            }
                            else
                            {
                                _rubikHolder.GetCurrentVisualizer().YPrime();
                                _cubeRotations.Add(new FaceRotation(CubeRotation.y, Rotation.Prime));
                            }
                            break;
                        case CubeRotation.x:
                            if (rotationType == Rotation.One)
                            {
                                _rubikHolder.GetCurrentVisualizer().X();
                                _cubeRotations.Add(new FaceRotation(CubeRotation.x, Rotation.One));
                            }
                            else
                            {
                                _rubikHolder.GetCurrentVisualizer().XPrime();
                                _cubeRotations.Add(new FaceRotation(CubeRotation.x, Rotation.Prime));
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    _currentPosition++;

                    rotationStep.SetColor(Color.blue);
                    rotationStep = _rotationSteps[_currentPosition % MAXMessageCount];
                }

                if (rotationStep.CheckCorrectTurn(rotation, _cubeRotations, ref _currentPosition))
                {
                    if (_currentPosition == _rotations.Count)
                        _rubikTimer.StopTimer();
                }
                else
                {
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
                else if (_currentPosition % MAXMessageCount == 0 && rotationStep.Finished)
                {
                    ShowNextBatch();
                }
            }
        }

        private IEnumerator PracticeFinishedCoroutine()
        {
            yield return new WaitForSeconds(2);
            if (!_animating)
                _eventBus.Invoke(new RotationsEmpty(_rubikTimer.TimeElapsed));
            _animating = false;
            AddPracticeModeRotations();
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
            _rotationSteps.Clear();
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
                AddPracticeModeRotations();
                _practiceMode = true;
            }
            _rotations = rotations.ToList();
            ShowNextBatch();
        }

        private void AddPracticeModeRotations()
        {
            _cubeRotations.Clear();
            _cubeRotations.Add(new FaceRotation(CubeRotation.x, Rotation.One));
            _cubeRotations.Add(new FaceRotation(CubeRotation.x, Rotation.One));
            _cubeRotations.Add(new FaceRotation(CubeRotation.y, Rotation.One));
        }

        private void ShowNextBatch()
        {
            DeleteMessageObjects();
            var endPosition = _currentPosition + MAXMessageCount > _rotations.Count ? _rotations.Count : _currentPosition + MAXMessageCount;
            for (var i = _currentPosition; i < endPosition; i++)
            {
                var createdMessage = Instantiate(_rotationMessagePrefab, _messagesParent);
                var rotationStep = createdMessage.GetComponent<RotationStep>();
                _rotationSteps.Add(rotationStep);
                rotationStep.LoadStep(_rotations[i]);
            }
        }
    }
}

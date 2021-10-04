using EventBus;
using EventBus.Events;
using Model;
using Parser;
using RubikVisualizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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
        private RubikCaseParser.RubikCase _currentCase;

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
                .Where(r => r.TurnType != TurnType.Cube);
            await _notificationParser.AnimateRotations(remainingRotations).ConfigureAwait(false);
        }

        private void NotificationParserOnFaceRotated(FaceRotated faceRotated)
        {
            var rotation = faceRotated.Rotation;
            _rubikTimer.StartTimer();
            if (_rotations.Count == 0 && _correctionTurns.Count == 0) return;
            if (_correctionTurns.Count != 0)
            {
                if (CheckCorrectTurn(_correctionTurns.Peek(), rotation))
                {
                    _correctionTurns.Pop();
                    Destroy(_wrongMessageParent.GetChild(0).gameObject);
                    if (_correctionTurns.Count == 0)
                    {
                        _messagesParent.GetChild(_currentPosition % MAXMessageCount).GetComponent<RotationStep>()
                            .ResetColor();
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

                var currentRotation = _rotations[_currentPosition];
                var currentVisualizer = _rubikHolder.GetCurrentVisualizer();
                if (currentRotation.TurnType == TurnType.Cube)
                {
                    _cubeRotations.Add(new FaceRotation(currentRotation.CubeRotation, currentRotation.RotationType));
                    var rotationType = currentRotation.RotationType;
                    switch (currentRotation.CubeRotation)
                    {
                        case CubeRotation.y:
                            if (rotationType == Rotation.One)
                            {
                                currentVisualizer.Y();
                            }
                            else
                            {
                                currentVisualizer.YPrime();
                            }

                            break;
                        case CubeRotation.x:
                            if (rotationType == Rotation.One)
                            {
                                currentVisualizer.X();
                            }
                            else
                            {
                                currentVisualizer.XPrime();
                            }

                            break;
                        case CubeRotation.z:
                            if (rotationType == Rotation.One)
                            {
                                currentVisualizer.Z();
                            }
                            else
                            {
                                currentVisualizer.ZPrime();
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    _currentPosition++;

                    rotationStep.SetColor(Color.blue);
                    rotationStep = _rotationSteps[_currentPosition % MAXMessageCount];
                }

                if (currentRotation.TurnType == TurnType.DoubleLayer)
                {
                    switch (currentRotation.DoubleLayerRotation)
                    {
                        case DoubleLayerRotation.u:
                            if (rotation.RotationType == Rotation.One)
                            {
                                currentVisualizer.Y();
                            }
                            else if (rotation.RotationType == Rotation.Prime)
                            {
                                currentVisualizer.YPrime();
                            }

                            break;
                        case DoubleLayerRotation.d:
                            if (rotation.RotationType == Rotation.One)
                            {
                                currentVisualizer.YPrime();
                            }
                            else if (rotation.RotationType == Rotation.Prime)
                            {
                                currentVisualizer.Y();
                            }

                            break;
                        case DoubleLayerRotation.r:
                            if (rotation.RotationType == Rotation.One)
                            {
                                currentVisualizer.X();
                            }
                            else if (rotation.RotationType == Rotation.Prime)
                            {
                                currentVisualizer.XPrime();
                            }

                            break;
                        case DoubleLayerRotation.l:
                            if (rotation.RotationType == Rotation.One)
                            {
                                currentVisualizer.XPrime();
                            }
                            else if (rotation.RotationType == Rotation.Prime)
                            {
                                currentVisualizer.X();
                            }

                            break;
                        case DoubleLayerRotation.f:
                            if (rotation.RotationType == Rotation.One)
                            {
                                currentVisualizer.Z();
                            }
                            else if (rotation.RotationType == Rotation.Prime)
                            {
                                currentVisualizer.ZPrime();
                            }

                            break;
                        case DoubleLayerRotation.b:
                            if (rotation.RotationType == Rotation.One)
                            {
                                currentVisualizer.ZPrime();
                            }
                            else if (rotation.RotationType == Rotation.Prime)
                            {
                                currentVisualizer.Z();
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
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
                        PracticeFinished().Forget();
                    }

                }
                else if (_currentPosition % MAXMessageCount == 0 && rotationStep.Finished)
                {
                    ShowNextBatch();
                }

            }
        }

        private async UniTask PracticeFinished()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            if (!_animating)
                _eventBus.Invoke(new RotationsEmpty(_rubikTimer.TimeElapsed));
            else if(_currentCase != null)
            {
                LoadCase(_currentCase, _practiceMode);
                _rubikHolder.LoadState(_currentCase.GetStateFromFaces(),_practiceMode);
            }
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

        public void LoadCase(RubikCaseParser.RubikCase rubikCase, bool f2LMode)
        {
            _currentCase = rubikCase;
            _rubikHolder.LoadState(_currentCase.GetStateFromFaces(), true);
            _currentPosition = 0;
            if (f2LMode)
            {
                AddPracticeModeRotations();
                _practiceMode = true;
            }
            _rotations = rubikCase.GetSolution().ToList();
            ShowNextBatch();
        }

        public void LoadRotations(IEnumerable<FaceRotation> rotations)
        {
            _currentPosition = 0;
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

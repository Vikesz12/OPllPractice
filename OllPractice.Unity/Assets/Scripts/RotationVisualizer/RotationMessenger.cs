using Ble;
using Cysharp.Threading.Tasks;
using EventBus;
using EventBus.Events;
using Model;
using Parser;
using RubikVisualizers;
using System;
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
        [Inject] private readonly IBle _ble;

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
        private bool _trainingMode;
        private bool _isHidden;

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
            if (_rotations.Count == 0 && _correctionTurns.Count == 0) return;

            var rotation = faceRotated.Rotation;

            if (!_trainingMode)
                _rubikTimer.StartTimer();

            if (_isHidden)
            {
                var isDone = false;
                switch (_currentCase.caseType)
                {
                    case TrainingMode.F2L:
                        if (_rubikHolder.GetCurrentVisualizer().IsF2LFinished())
                        {
                            isDone = true;
                        }
                        break;
                    case TrainingMode.Pll:
                        if (_rubikHolder.GetCurrentVisualizer().IsPllFinished())
                        {
                            isDone = true;
                        }
                        break;
                    case TrainingMode.Oll:
                        if (_rubikHolder.GetCurrentVisualizer().IsOllFinished())
                        {
                            isDone = true;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (!isDone) return;

                _rubikTimer.StopTimer();
                _currentPosition = 0;
                _isHidden = false;
                PracticeFinished().Forget();
                return;
            }

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
                    RotateCube(currentRotation, rotation, currentVisualizer);
                    _currentPosition++;
                    rotationStep.SetColor(Color.blue);
                    rotationStep = _rotationSteps[_currentPosition % MAXMessageCount];
                }

                if (rotationStep.CheckCorrectTurn(rotation, _cubeRotations, ref _currentPosition))
                {
                    if (currentRotation.TurnType == TurnType.DoubleLayer)
                    {
                        DoubleLayerRotateCube(currentRotation, rotation, currentVisualizer);
                    }

                    if (_currentPosition == _rotations.Count)
                    {
                        _rubikTimer.StopTimer();
                        if (!_practiceMode && !_trainingMode  && !_isHidden)
                        {
                            Clear();
                        }
                        else if (_trainingMode)
                        {
                            _isHidden = true;
                            LoadCase(_currentCase, true);
                            _trainingMode = false;
                        }
                        else
                        {
                            _currentPosition = 0;
                            PracticeFinished().Forget();
                        }
                    }
                }
                else
                {
                    AddCorrectionTurnFor(rotation);
                }

                if (_currentPosition % MAXMessageCount == 0 && rotationStep.Finished)
                {
                    ShowNextBatch();
                }
            }
        }

        private void RotateCube(FaceRotation currentRotation, FaceRotation rotation, RubikVisualizer currentVisualizer)
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
        }
        private void DoubleLayerRotateCube(FaceRotation currentRotation, FaceRotation rotation, RubikVisualizer currentVisualizer)
        {
            CubeRotation cubeRotation;
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

                    cubeRotation = CubeRotation.y;
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

                    cubeRotation = CubeRotation.y;
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

                    cubeRotation = CubeRotation.x;
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

                    cubeRotation = CubeRotation.x;
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

                    cubeRotation = CubeRotation.z;
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

                    cubeRotation = CubeRotation.z;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _cubeRotations.Add(new FaceRotation(cubeRotation, rotation.RotationType));
        }

        private async UniTask PracticeFinished()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            if (!_animating)
                _eventBus.Invoke(new RotationsEmpty(_rubikTimer.TimeElapsed));
            else if (_currentCase != null)
            {
                LoadCase(_currentCase, _practiceMode);
                _rubikHolder.LoadState(_currentCase.GetStateFromFaces(), _practiceMode);
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
            if (!_isHidden && !_trainingMode)
            {
                _rubikHolder.LoadState(_currentCase.GetStateFromFaces(), true);
            }

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
                rotationStep.LoadStep(_rotations[i],_isHidden);
            }
        }

        public void LoadScramble(RubikCaseParser.RubikCase currentCase, bool flipMode)
        {
            _currentCase = currentCase;
            _ble.Write("3", ConnectedDeviceData.ConnectedDeviceId);
            _currentPosition = 0;
            if (flipMode)
            {
                _rubikHolder.Flip();
                AddPracticeModeRotations();
                _trainingMode = true;
            }
            _rotations = currentCase.GetScramble().Select(fr => fr.ToCubeTurnedRotation(_cubeRotations)).ToList();
            ShowNextBatch();
        }
    }
}

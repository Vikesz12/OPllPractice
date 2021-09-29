using System;
using Parser;
using RotationVisualizer;
using RubikVisualizers;
using UnityEngine;

namespace Loaders
{
    public class RubikCaseLoader : MonoBehaviour
    {
        [SerializeField] private RubikHolder _rubikHolder;
        [SerializeField] private RotationMessenger _rotationMessenger;

        private void Start()
        {
            LoadRandomCase();
            _rotationMessenger.PracticeFinished += PracticeFinished;
        }

        private void OnDestroy() => _rotationMessenger.PracticeFinished -= PracticeFinished;

        private void PracticeFinished() => LoadRandomCase();

        private void LoadRandomCase()
        {
            var randomCase = SelectedRubikCases.GetRandomCase();
            _rotationMessenger.LoadRotations(randomCase.GetSolution(), true);
            _rubikHolder.LoadState(randomCase.GetStateFromFaces(), true);
        }
    }
}

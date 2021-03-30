using System.Collections.Generic;
using Parser;
using RotationVisualizer;
using RubikVisualizers;
using UnityEngine;

namespace Loaders
{
    public class F2LLoader : MonoBehaviour
    {
        [SerializeField] private RubikHolder _rubikHolder;
        [SerializeField] private RotationMessenger _rotationMessenger;
        private F2LCaseParser _f2lParser;
        private List<F2LCaseParser.F2LCase> _cases;

        public void Start()
        {
            _f2lParser = new F2LCaseParser();
        }

        public void LoadF2LCase()
        {
            _cases = F2LCaseParser.LoadJson();
            _rubikHolder.LoadState(_cases[0].GetStateFromFaces());
            _rubikHolder.Flip();
            _rotationMessenger.LoadRotations(_cases[0].GetSolution(),true);
            _rotationMessenger.PracticeFinished += PracticeFinished;
        }

        private void PracticeFinished()
        {
            _rubikHolder.ResetVisualizer();
            _rubikHolder.LoadState(_cases[0].GetStateFromFaces());
            _rubikHolder.Flip();
        }
    }
}

using System.Collections.Generic;
using Parser;
using RotationVisualizer;
using RubikVisualizers;
using UnityEngine;

namespace Loaders
{
    public class F2LLoader : MonoBehaviour
    {
        [SerializeField] private RubikVisualizer _rubikVisualizer;
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
            _rubikVisualizer.LoadState(_cases[0].GetStateFromFaces());
            _rubikVisualizer.Flip();
            _rotationMessenger.LoadRotations(_cases[0].GetSolution(),true);
        }

    }
}

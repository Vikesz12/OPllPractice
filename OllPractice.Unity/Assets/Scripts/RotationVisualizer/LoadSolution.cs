using RubikVisualizers;
using Solver;
using UnityEngine;

namespace RotationVisualizer
{
    public class LoadSolution : MonoBehaviour
    {
        [SerializeField] private RotationMessenger _rotationMessenger;
        [SerializeField] private RubikHolder _rubikHolder;

        public void LoadSolutionToMessenger() 
            => _rotationMessenger
                .LoadRotations(KociembaSolver.SolveCube(_rubikHolder.GetCurrentVisualizer().GetCurrentCube.GetFaces), false);
    }
}

using Model;
using UnityEngine;

namespace RubikVisualizers
{
    public class RubikHolder : MonoBehaviour
    {
        private RubikVisualizer _currentVisualizer;
        private bool _flipped;

        public void Awake() => CreateVisualizer();

        private void CreateVisualizer() => _currentVisualizer =
            Instantiate(Resources.Load<GameObject>("Prefabs/RubiksConnected"), transform)
            .GetComponent<RubikVisualizer>();


        public void LoadState(Face[] state)
        {
            ResetVisualizer();
            _currentVisualizer.LoadState(state);
        }

        public void Flip()
        {
            if (_flipped) return;
            _flipped = true;
            _currentVisualizer.Flip();
        }

        public void ResetVisualizer()
        {
            Destroy(_currentVisualizer.gameObject);
            _flipped = false;
            CreateVisualizer();
        }

        public RubikVisualizer GetCurrentVisualizer() => _currentVisualizer;
    }
}

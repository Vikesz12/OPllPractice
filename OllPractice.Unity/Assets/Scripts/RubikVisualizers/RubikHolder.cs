using Model;
using UnityEngine;
using Zenject;

namespace RubikVisualizers
{
    public class RubikHolder : MonoBehaviour
    {
        [Inject] private DiContainer _container;

        private RubikVisualizer _currentVisualizer;
        private bool _flipped;

        private void Start()
        {
            if(_currentVisualizer != null) return;
            CreateVisualizer();
        }

        private void OnDestroy()
        {
            Destroy(_currentVisualizer.gameObject);
            _currentVisualizer = null;
        }

        private void CreateVisualizer() =>
            _currentVisualizer =
                _container.InstantiatePrefab(Resources.Load<GameObject>("Prefabs/RubiksConnected"), transform)
                    .GetComponent<RubikVisualizer>();


        public void LoadState(Face[] state, bool flip = false)
        {
            ResetVisualizer();
            _currentVisualizer.LoadState(state);
            if(flip)
                _currentVisualizer.Flip();
        }

        public void Flip()
        {
            if (_flipped) return;
            _flipped = true;
            if(_currentVisualizer != null)
                _currentVisualizer.Flip();
        }

        private void ResetVisualizer()
        {
            if (_currentVisualizer != null)
            {
                Destroy(_currentVisualizer.gameObject);
            }

            _flipped = false;
            CreateVisualizer();
        }

        public RubikVisualizer GetCurrentVisualizer() => _currentVisualizer;
    }
}

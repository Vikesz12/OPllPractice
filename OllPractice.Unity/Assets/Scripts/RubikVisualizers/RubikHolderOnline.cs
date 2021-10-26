using UnityEngine;
using Zenject;

namespace RubikVisualizers
{
    public class RubikHolderOnline : MonoBehaviour
    {
        [Inject] private DiContainer _container;

        private RubikVisualizer _currentVisualizer;
        private bool _flipped;

        private void Start()
        {
            if (_currentVisualizer != null) return;
            CreateVisualizer();
        }

        private void OnDestroy()
        {
            Destroy(_currentVisualizer.gameObject);
            _currentVisualizer = null;
        }

        private void CreateVisualizer() =>
            _currentVisualizer =
                _container.InstantiatePrefab(Resources.Load<GameObject>("Prefabs/RubiksConnectedOnline"), transform)
                    .GetComponent<RubikVisualizer>();
        public RubikVisualizer GetCurrentVisualizer() => _currentVisualizer;

    }
}
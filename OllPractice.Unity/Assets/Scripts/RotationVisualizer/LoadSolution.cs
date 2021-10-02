using Ble;
using EventBus;
using EventBus.Events;
using Solver;
using UnityEngine;
using Zenject;

namespace RotationVisualizer
{
    public class LoadSolution : MonoBehaviour
    {
        [Inject] private readonly IBle _ble;
        [Inject] private readonly IEventBus _eventBus;

        [SerializeField] private RotationMessenger _rotationMessenger;


        private void Awake() => _eventBus.Subscribe<StateParsed>(OnStateLoaded);

        private void Start() => _ble.Write("3", ConnectedDeviceData.ConnectedDeviceId);

        private void OnDestroy() => _eventBus.Unsubscribe<StateParsed>(OnStateLoaded);

        private void OnStateLoaded(StateParsed parsed) =>
            _rotationMessenger
                .LoadRotations(KociembaSolver.SolveCube(parsed.Faces), false);
    }
}

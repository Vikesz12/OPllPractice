using Ble;
using EventBus;
using EventBus.Events;
using Solver;
using System.Collections;
using UnityEngine;
using Zenject;

namespace RotationVisualizer
{
    public class LoadSolution : MonoBehaviour
    {
        [Inject] private IBle _ble;
        [Inject] private IEventBus _eventBus;

        [SerializeField] private RotationMessenger _rotationMessenger;

        private bool _tryParse;

        private void Awake()
        {
            _eventBus.Subscribe<StateParsed>(OnStateLoaded);
            StartStateRequest();
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<StateParsed>(OnStateLoaded);
            _tryParse = false;
        }

        private void StartStateRequest()
        {
            _tryParse = true;
            StartCoroutine(RequestState());
        }

        private IEnumerator RequestState()
        {
            while (_tryParse)
            {
                yield return new WaitForSeconds(3);

                _ble.Write("3", ConnectedDeviceData.ConnectedDeviceId);
            }
        }


        private void OnStateLoaded(StateParsed parsed)
        {
            _tryParse = false;
            _rotationMessenger
                .LoadRotations(KociembaSolver.SolveCube(parsed.Faces), false);
        }
    }
}

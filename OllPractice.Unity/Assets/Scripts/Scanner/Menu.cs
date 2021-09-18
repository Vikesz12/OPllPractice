using Ble;
using System.Collections;
using EventBus;
using EventBus.Events;
using UnityEngine;
using Zenject;

namespace Scanner
{
    public class Menu : MonoBehaviour
    {
        [Inject] private IBle _bleScanner;
        [Inject] private IEventBus _eventBus;

        private string _connectedDeviceId;
        private bool _tryParse;

        private void Start()
        {
            _eventBus.Subscribe<StateParsed>(StateParsedHandler);
            _eventBus.Subscribe<ConnectedToDevice>(RequestState);
        }

        private void StateParsedHandler(StateParsed parsed)
        {
            _tryParse = false;
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        private void RequestState(ConnectedToDevice device)
        {
            _connectedDeviceId = device.DeviceId;
            _tryParse = true;
            StartCoroutine(RequestState());
        }

        private IEnumerator RequestState()
        {
            while (_tryParse)
            {
                yield return new WaitForSeconds(3);

                _bleScanner.Write("3", _connectedDeviceId);
            }
        }
    }
}

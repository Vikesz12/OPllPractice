using System;
using System.Collections;
using Ble;
using Events;
using UnityEngine;
using Zenject;

namespace Scanner
{
    public class Menu : MonoBehaviour
    {
        [Inject] private IBle _bleScanner;

        private string _connectedDeviceId;
        private bool _tryParse;

        private void Start()
        {
            EventBus.Instance.Value.Subscribe<StateParsed>(StateParsedHandler);
            EventBus.Instance.Value.Subscribe<ConnectedToDevice>(RequestState);
        }

        private void StateParsedHandler(StateParsed parsed)
        {
            _tryParse = false;
            gameObject.SetActive(false);
            EventBus.Instance.Value.Unsubscribe<StateParsed>(StateParsedHandler);
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
                _bleScanner.Write("3", _connectedDeviceId);

                yield return new WaitForSeconds(5);
            }
        }
    }
}

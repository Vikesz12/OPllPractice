using Ble;
using UnityEngine;
using Zenject;

namespace Online
{
    public class LoadState : MonoBehaviour
    {
        [Inject] private readonly IBle _ble;

        private void Start() => _ble.Write("3", ConnectedDeviceData.ConnectedDeviceId);
    }
}

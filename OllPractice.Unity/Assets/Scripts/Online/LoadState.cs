using Ble;
using Mirror;
using RubikVisualizers;
using Zenject;

namespace Online
{
    public class LoadState : NetworkBehaviour
    {
        [Inject] private readonly IBle _ble;
        [Inject] private DiContainer _container;

        private void Start()
        {
            if (isLocalPlayer)
            {
                _container.InstantiateComponent<RubikHolder>(gameObject);
                gameObject.GetComponent<RubikOnlineMessageSender>().enabled = true;
            }
            else
            {
                _container.InstantiateComponent<RubikHolderOnline>(gameObject);
            }

            _ble.Write("3", ConnectedDeviceData.ConnectedDeviceId);
        }
    }
}

using Ble;
using Mirror;
using RubikVisualizers;
using Zenject;

namespace Online
{
    public class LoadState : NetworkBehaviour
    {
        [Inject] private readonly IBle _ble;

        private void  Start()
        {
            var rubikHolder = GetComponent<RubikHolder>();

            if (isLocalPlayer)
            {
                _ble.Write("3", ConnectedDeviceData.ConnectedDeviceId);
            }
            else
            {
                rubikHolder.SetOnline(true);
            }

            rubikHolder.enabled = true;
        }
    }
}

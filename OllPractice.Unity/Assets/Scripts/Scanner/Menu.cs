using Ble;
using EventBus;
using EventBus.Events;
using UnityEngine;
using Zenject;

namespace Scanner
{
    public class Menu : MonoBehaviour
    {
        [Inject] private IEventBus _eventBus;

        [SerializeField] private GameObject _scanMenu;
        [SerializeField] private GameObject _mainMenu;


        private void Start()
        {
            if(ConnectedDeviceData.ConnectedDeviceId != null) ShowMainMenu();
            _eventBus.Subscribe<ConnectedToDevice>(OnConnected);
        }

        private void OnDestroy() => _eventBus.Unsubscribe<ConnectedToDevice>(OnConnected);

        private void OnConnected(ConnectedToDevice device) => ShowMainMenu();

        private void ShowMainMenu()
        {
            _scanMenu.SetActive(false);
            _mainMenu.SetActive(true);
        }
    }
}

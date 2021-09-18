using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    [RequireComponent(typeof(Button))]
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private GameObject _toActivate;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ShowNewMenu);
        }

        private void OnDestroy() => _button.onClick.RemoveListener(ShowNewMenu);

        private void ShowNewMenu()
        {
            _toActivate.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        }
    }
}

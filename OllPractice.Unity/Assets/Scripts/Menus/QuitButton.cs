using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    [RequireComponent(typeof(Button))]
    public class QuitButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(QuitApp);
        }

        private void OnDestroy() => _button.onClick.RemoveListener(QuitApp);

        private void QuitApp() => Application.Quit();
    }
}

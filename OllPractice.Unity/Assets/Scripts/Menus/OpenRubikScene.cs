using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menus
{
    [RequireComponent(typeof(Button))]
    public class OpenRubikScene : MonoBehaviour
    {
        [SerializeField] private int _sceneIndex;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OpenScene);
        }

        private void OpenScene() => SceneManager.LoadScene(_sceneIndex);
    }
}
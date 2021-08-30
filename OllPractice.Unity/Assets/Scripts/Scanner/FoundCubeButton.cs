using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scanner
{
    public class FoundCubeButton : MonoBehaviour
    {
        [SerializeField] private Button _cubeButton;
        [SerializeField] private TextMeshProUGUI _cubeNameText;

        public void SetCubeName(string cubeName) => _cubeNameText.text = cubeName;
        public Button GetButton => _cubeButton;
    }
}

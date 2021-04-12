using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Loaders
{
    public class F2LListElementLoader : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _caseName;
        private int _caseNumber;
        private F2LLoader _loader;
        public void LoadCase(int caseNumber, string caseName, F2LLoader f2LLoader)
        {
            _caseName.text = caseName;
            _caseNumber = caseNumber;
            _loader = f2LLoader;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _loader.LoadF2LCase(_caseNumber);
            _loader.gameObject.SetActive(false);
        }
    }
}

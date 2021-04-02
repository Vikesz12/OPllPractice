using Parser;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Loaders
{
    public class F2LListElementLoader : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _caseName;
        private F2LCaseParser.F2LCase _currentCase;
        private F2LLoader _loader;
        public void LoadCase(F2LCaseParser.F2LCase currentCase, F2LLoader f2LLoader)
        {
            _caseName.text = currentCase.name;
            _currentCase = currentCase;
            _loader = f2LLoader;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _loader.LoadF2LCase(_currentCase.GetStateFromFaces(), _currentCase.GetSolution());
        }
    }
}

using Parser;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Loaders
{
    public class RubikListElementLoader : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _caseName;
        [SerializeField] private Toggle _toggle;

        private int _caseNumber;
        private RubikCaseParser.RubikCase _currentCase;

        public void LoadCase(int caseNumber, RubikCaseParser.RubikCase rubikCase)
        {
            _currentCase = rubikCase;
            _caseName.text = rubikCase.name;
            _caseNumber = caseNumber;

            _toggle.onValueChanged.AddListener(OnToggleClick);
            OnToggleClick(_toggle.isOn);
        }

        private void OnToggleClick(bool selected)
        {
            if (selected)
            {
                SelectedRubikCases.AddCase(_currentCase);
            }
            else
            {
                SelectedRubikCases.RemoveCase(_currentCase);
            }
        }
    }
}

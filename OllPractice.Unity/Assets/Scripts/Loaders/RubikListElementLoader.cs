using System;
using Parser;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Loaders
{
    public class RubikListElementLoader : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _caseName;
        [SerializeField] private TextMeshProUGUI _averageTime;
        [SerializeField] private TextMeshProUGUI _bestTime;
        [SerializeField] private TMP_InputField _solutionField;
        [SerializeField] private Image _image;
        [SerializeField] private Toggle _toggle;
        [SerializeField] private Button _saveButton;

        private RubikCaseParser.RubikCase _currentCase;

        private void Start() => _saveButton.onClick.AddListener(UpdateCase);

        private void UpdateCase()
        {
            _currentCase.solution = _solutionField.text;
            GetComponentInParent<RubikCaseListLoader>().UpdateCase(_currentCase);
        }

        private void OnDestroy()
        {
            _toggle.onValueChanged.RemoveAllListeners();
            _saveButton.onClick.RemoveListener(UpdateCase);
        }

        public void LoadCase(RubikCaseParser.RubikCase rubikCase, bool isPractice)
        {
            _currentCase = rubikCase;
            _caseName.text = rubikCase.name;
            _solutionField.text = rubikCase.solution;
            _image.sprite = Resources.Load<Sprite>($"Images/{_currentCase.caseType.ToString().ToUpperInvariant()}/{_currentCase.name}");

            var stats = isPractice ? RubikStats.PracticeStats : RubikStats.TrainingStats;
            var currentStat = stats.cases.Find(c => c.name == rubikCase.name);
            if (currentStat != null)
            {
                _averageTime.text = "Avg: " + currentStat.average.ToString("F", CultureInfo.InvariantCulture);
                _bestTime.text = "Best: " + currentStat.best.ToString("F", CultureInfo.InvariantCulture);
            }


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

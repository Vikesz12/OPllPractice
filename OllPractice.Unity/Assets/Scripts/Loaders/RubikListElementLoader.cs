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
        [SerializeField] private Image _image;
        [SerializeField] private Toggle _toggle;

        private RubikCaseParser.RubikCase _currentCase;

        public void LoadCase(RubikCaseParser.RubikCase rubikCase, bool isPractice)
        {
            _currentCase = rubikCase;
            _caseName.text = rubikCase.name;
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

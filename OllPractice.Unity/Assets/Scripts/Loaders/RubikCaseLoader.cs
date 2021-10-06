using EventBus;
using EventBus.Events;
using Parser;
using RotationVisualizer;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Loaders
{
    public class RubikCaseLoader : MonoBehaviour
    {
        [Inject] private readonly IEventBus _eventBus;

        [SerializeField] private RotationMessenger _rotationMessenger;
        [SerializeField] private bool _isPractice;

        private RubikCaseParser.RubikCase _currentCase;

        private void Start()
        {
            LoadRandomCase();
            _eventBus.Subscribe<RotationsEmpty>(PracticeFinished);
        }

        private void OnDestroy() => _eventBus.Unsubscribe<RotationsEmpty>(PracticeFinished);

        private void PracticeFinished(RotationsEmpty rotationsEmpty)
        {
            SaveTime(rotationsEmpty.FinishTime);
            LoadRandomCase();
        }

        private void SaveTime(float solveTime)
        {
            var stats = _isPractice ? RubikStats.PracticeStats : RubikStats.TrainingStats;

            var rubikCaseStat = stats.cases.FirstOrDefault(c => c.name == _currentCase.name);
            if (rubikCaseStat == null)
            {
                rubikCaseStat = new RubikCaseStat { name = _currentCase.name };
                stats.cases.Add(rubikCaseStat);
            }
            if (rubikCaseStat.solveCount == 0)
            {
                rubikCaseStat.average = solveTime;
                rubikCaseStat.best = solveTime;
            }
            else
            {
                rubikCaseStat.average = (rubikCaseStat.average * rubikCaseStat.solveCount + solveTime) /
                    (rubikCaseStat.solveCount + 1);
                if (solveTime < rubikCaseStat.best)
                    rubikCaseStat.best = solveTime;
            }
            rubikCaseStat.solveCount++;

            stats.cases[stats.cases.FindIndex(c => c.name == rubikCaseStat.name)] = rubikCaseStat;
            RubikStats.SaveData();
        }

        private void LoadRandomCase()
        {
            _currentCase = SelectedRubikCases.GetRandomCase();
            if (_isPractice)
                _rotationMessenger.LoadCase(_currentCase, true);
            else
                _rotationMessenger.LoadScramble(_currentCase, true);
        }
    }
}

using System.Collections.Generic;
using Model;
using Parser;
using RotationVisualizer;
using RubikVisualizers;
using UnityEngine;

namespace Loaders
{
    public class F2LLoader : MonoBehaviour
    {
        [SerializeField] private RubikHolder _rubikHolder;
        [SerializeField] private RotationMessenger _rotationMessenger;
        [SerializeField] private Transform _listParent;
        private List<F2LCaseParser.F2LCase> _cases;

        public void Start()
        {
            _cases = F2LCaseParser.LoadJson();
            InstantiateListElements();
            gameObject.SetActive(false);
        }

        private void InstantiateListElements()
        {
            var listPrefab = Resources.Load<GameObject>("Prefabs/F2LListElement");
            for (var i = 0; i < _cases.Count; i++)
            {
                var f2LCase = _cases[i];
                Instantiate(listPrefab, _listParent).GetComponent<F2LListElementLoader>().LoadCase(i, f2LCase.name, this);
            }
        }

        public void LoadF2LCase(int caseNumber)
        {
            _rubikHolder.LoadState(_cases[caseNumber].GetStateFromFaces());
            _rubikHolder.Flip();
            _rotationMessenger.LoadRotations(_cases[caseNumber].GetSolution(),true);
            _rotationMessenger.PracticeFinished += () => PracticeFinished(_cases[caseNumber].GetStateFromFaces());
        }

        private void PracticeFinished(Face[] state)
        {
            //_rubikHolder.ResetVisualizer();
            _rubikHolder.LoadState(state);
            _rubikHolder.Flip();
        }
    }
}

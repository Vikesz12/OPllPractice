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
            foreach (var f2LCase in _cases)
            {
                Instantiate(listPrefab,_listParent).GetComponent<F2LListElementLoader>().LoadCase(f2LCase, this);
            }
        }

        public void LoadF2LCase(Face[] state, List<FaceRotation> solution)
        {
            _rubikHolder.LoadState(state);
            _rubikHolder.Flip();
            _rotationMessenger.LoadRotations(solution,true);
            _rotationMessenger.PracticeFinished += PracticeFinished;
        }

        private void PracticeFinished()
        {
            _rubikHolder.ResetVisualizer();
            _rubikHolder.LoadState(_cases[0].GetStateFromFaces());
            _rubikHolder.Flip();
        }
    }
}

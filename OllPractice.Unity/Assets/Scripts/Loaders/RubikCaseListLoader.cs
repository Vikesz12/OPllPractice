﻿using Parser;
using System.Collections.Generic;
using UnityEngine;

namespace Loaders
{
    public class RubikCaseListLoader : MonoBehaviour
    {
        [SerializeField] private Transform _listParent;
        [SerializeField] private bool _isPractice;

        private List<RubikCaseParser.RubikCase> _cases;

        private void Awake()
        {
            _cases = RubikCaseParser.LoadJson();
            SelectedRubikCases.ClearCases();
            RubikStats.LoadData();
            InstantiateListElements(_isPractice);
            gameObject.SetActive(false);
        }

        private void InstantiateListElements(bool isPractice)
        {
            var listPrefab = Resources.Load<GameObject>("Prefabs/F2LListElement");
            foreach (var f2LCase in _cases)
            {
                Instantiate(listPrefab, _listParent).GetComponent<RubikListElementLoader>().LoadCase(f2LCase, isPractice);
            }
        }
    }
}

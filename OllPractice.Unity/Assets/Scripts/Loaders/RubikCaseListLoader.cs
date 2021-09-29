using Parser;
using System.Collections.Generic;
using UnityEngine;

namespace Loaders
{
    public class RubikCaseListLoader : MonoBehaviour
    {
        [SerializeField] private Transform _listParent;
        private List<RubikCaseParser.RubikCase> _cases;

        private void Awake()
        {
            _cases = RubikCaseParser.LoadJson();
            InstantiateListElements();
            gameObject.SetActive(false);
        }

        private void InstantiateListElements()
        {
            var listPrefab = Resources.Load<GameObject>("Prefabs/F2LListElement");
            for (var i = 0; i < _cases.Count; i++)
            {
                var f2LCase = _cases[i];
                Instantiate(listPrefab, _listParent).GetComponent<RubikListElementLoader>().LoadCase(i, f2LCase);
            }
        }
    }
}

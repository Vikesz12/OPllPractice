using System.Collections.Generic;
using Parser;
using UnityEngine;

namespace Loaders
{
    public class F2LLoader : MonoBehaviour
    {
        private F2LCaseParser _f2lParser;
        private List<F2LCaseParser.F2LCase> _cases;

        public void Start()
        {
            _f2lParser = new F2LCaseParser();
        }

        public void LoadF2LCase()
        {
            _cases = F2LCaseParser.LoadJson();
        }

    }
}

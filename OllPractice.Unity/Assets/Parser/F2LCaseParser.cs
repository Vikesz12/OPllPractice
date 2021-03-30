using Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Parser
{
    public sealed class F2LCaseParser
    {
        public static List<F2LCase> LoadJson()
        {
            var jsonFile = Resources.Load<TextAsset>("Cases/F2LCases");

            return JsonUtility.FromJson<F2LCaseList>(jsonFile.text).F2LCases;

        }

        [Serializable]
        private sealed class F2LCaseList
        {
            public List<F2LCase> F2LCases;
        }
        [Serializable]
        public sealed class F2LCase
        {
            public string Name;
            public List<string> Faces;
            public string Solution;
            public List<Face> GetStateFromFaces()
            {
                throw new NotImplementedException();
            }

            public List<Rotation> GetSolution()
            {
                throw new NotImplementedException();
            }
        }
    }
}

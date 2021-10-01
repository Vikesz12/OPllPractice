using Model;
using RubikVisualizers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Parser
{
    public sealed class RubikCaseParser
    {
        public static List<RubikCase> LoadJson(string jsonName)
        {
            var jsonFile = Resources.Load<TextAsset>("Cases/" + jsonName);

            return JsonUtility.FromJson<RubikCaseList>(jsonFile.text).rubikCases;

        }

        [Serializable]
        private sealed class RubikCaseList
        {
            public List<RubikCase> rubikCases;
        }

        [Serializable]
        public sealed class RubikCase
        {
            public string name;
            public List<string> faces;
            public string solution;

            public Face[] GetStateFromFaces()
            {
                var result = new Face[6];
                for (var i = 0; i < faces.Count; i++)
                {
                    var face = faces[i];
                    var faceResult = new RubikColor[8];
                    for (var index = 0; index < face.Length; index++)
                    {
                        var character = face[index];
                        Enum.TryParse($"{character}", out RubikColor color);
                        faceResult[index] = color;
                    }

                    result[i] = new Face(faceResult.Reverse());
                }

                return result;
            }

            public List<FaceRotation> GetSolution()
            {
                var notes = solution.Split(',');

                return notes.Select(n => new FaceRotation(n)).ToList();
            }
        }
    }
}

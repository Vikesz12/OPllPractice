﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RubikVisualizers;
using UnityEngine;

namespace Parser
{
    public sealed class F2LCaseParser
    {
        public static List<F2LCase> LoadJson()
        {
            var jsonFile = Resources.Load<TextAsset>("Cases/F2LCases");

            return JsonUtility.FromJson<F2LCaseList>(jsonFile.text).f2LCases;

        }

        [Serializable]
        private sealed class F2LCaseList
        {
            public List<F2LCase> f2LCases;
        }
        [Serializable]
        public sealed class F2LCase
        {
            public string name;
            public List<string> faces;
            public string solution;

            public Face[] GetStateFromFaces()
            {
                var result = new Face[6];
                for (var index1 = 0; index1 < faces.Count; index1++)
                {
                    var face = faces[index1];
                    var faceResult = new RubikColor[8];
                    for (var index = 0; index < face.Length; index++)
                    {
                        var character = face[index];
                        Enum.TryParse($"{character}", out RubikColor color);
                        faceResult[index] = color;
                    }

                    result[index1] = new Face(faceResult.Reverse());
                }

                return result;
            }

            public List<FaceRotation> GetSolution()
            {
                var notes = solution.Split(',');
                var result = new List<FaceRotation>();
                foreach (var note in notes)
                {
                    var faceRotationString = note.Replace("'", "Prime");
                    Enum.TryParse(faceRotationString, out FaceRotation rotationToAdd);
                    result.Add(rotationToAdd);
                }

                return result;
            }
        }
    }
}

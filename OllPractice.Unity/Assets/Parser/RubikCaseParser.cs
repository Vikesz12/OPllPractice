using Model;
using RotationVisualizer;
using Solver;
using System;
using System.Collections.Generic;
using System.IO;
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

        public static void SaveJson(string jsonName, RubikCaseList cases)
        {
            var fileText = JsonUtility.ToJson(cases);
            File.WriteAllText(Application.dataPath + "/Resources/Cases/" + jsonName +".json", fileText);
        }

        [Serializable]
        public sealed class RubikCaseList
        {
            public List<RubikCase> rubikCases;

            public RubikCaseList(List<RubikCase> rubikCases) => this.rubikCases = rubikCases;
        }

        [Serializable]
        public sealed class RubikCase
        {
            public string name;
            public List<string> faces;
            public string solution;
            public TrainingMode caseType;

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

            public List<FaceRotation> GetScramble()
            {
                var cube = new Cube();
                var cubeRotations = new List<FaceRotation>
                {
                    new FaceRotation(CubeRotation.x, Rotation.One),
                    new FaceRotation(CubeRotation.x, Rotation.One),
                    new FaceRotation(CubeRotation.y, Rotation.One)
                };
                var alg = GetSolution();
                alg.ReverseAlg();
                cube.DoAlgorithm(cubeRotations.ReverseCubeRotations(), alg);
                var caseSolution = KociembaSolver.SolveCube(cube.GetFaces).ToList();
                caseSolution.ReverseAlg();
                return caseSolution;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Model;
using RubikVisualizers;
using UnityEngine;
using UnityEngine.UI;

namespace JsonTool
{
    [RequireComponent(typeof(Button))]
    public class PrintState : MonoBehaviour
    {
        [SerializeField] private RubikVisualizer _visualizer;

        private void Awake() => GetComponent<Button>().onClick.AddListener(PrintCurrentState);

        private void PrintCurrentState()
        {
            var lines = new List<string>();

            foreach (var face in _visualizer.GetFaces)
            {
                var line = new List<RubikColor>(new RubikColor[8]);
                foreach (var faceCube in face.Cubes)
                {
                    var i = face.GetCubeIndex(faceCube.transform.position);
                    line[i] = faceCube.GetComponent<ISetFaceColor>().GetFaceColorForFacing(face.Facing);
                }
                lines.Add(line.Aggregate(string.Empty,(current, color) => current + color));
            }

            Debug.Log(lines.Skip(1).Aggregate($"\"{lines[0]}\"",(a,b) => $"{a},{Environment.NewLine}\"{b}\""));
        }
    }
}

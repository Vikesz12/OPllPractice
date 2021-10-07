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
            var lines = _visualizer.GetCurrentCube.GetFaces
                .Select(face => face.GetAllColors()
                    .Aggregate(string.Empty, (current, color) => current + (RubikColor)color))
                .ToList();
            Debug.Log(lines.Skip(1).Aggregate($"\"{lines[0]}\"",(a,b) => $"{a},{Environment.NewLine}\"{b}\""));
        }
    }
}

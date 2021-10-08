using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Model;
using RubikVisualizers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JsonTool
{
    [RequireComponent(typeof(Button))]
    public class PrintState : MonoBehaviour
    {
        [SerializeField] private RubikVisualizer _visualizer;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TMP_InputField _solutionField;
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private Camera _camera;

        private void Awake() => GetComponent<Button>().onClick.AddListener(PrintCurrentState);

        private void PrintCurrentState()
        {
            SaveImage();
            var lines = new List<string>();

            foreach (var face in _visualizer.GetFaces)
            {
                var line = new List<RubikColor>(new RubikColor[8]);
                foreach (var faceCube in face.Cubes)
                {
                    var i = face.GetCubeIndex(faceCube.transform.position);
                    line[i] = faceCube.GetComponent<ISetFaceColor>().GetFaceColorForFacing(face.Facing);
                }
                lines.Add(line.Aggregate(string.Empty, (current, color) => current + color));
            }

            var solutionText = ParseSolutionText();
            var typeInt = ParseTypeInt();
            Debug.Log($"{{{Environment.NewLine}"
                      + $"\"name\": \"{_inputField.text}\","
                      + $"{Environment.NewLine} \"faces\": [{Environment.NewLine}"
                      + lines.Skip(1).Aggregate($"\"{lines[0]}\"", (a, b) => $"{a},{Environment.NewLine}\"{b}\"")
                      + $"{Environment.NewLine}],{Environment.NewLine}\"solution\": \"{solutionText}\","
                      + $"{Environment.NewLine}\"caseType\": {typeInt}"
                      + $"{Environment.NewLine}}},");
        }

        private int ParseTypeInt() =>
            _dropdown.options[_dropdown.value].text switch
            {
                "F2L" => 0,
                "OLL" => 1,
                "PLL" => 2,
                _ => 0
            };

        private string ParseSolutionText()
        {
            var result = _solutionField.text;
            result = result.Replace("(", string.Empty).Replace(")", string.Empty);
            result = result.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            return result.Replace(' ', ',');
        }

        private void SaveImage()
        {
            var activeRenderTexture = RenderTexture.active;
            RenderTexture.active = _camera.targetTexture;

            _camera.Render();

            var targetTexture = _camera.targetTexture;
            var image = new Texture2D(targetTexture.width, targetTexture.height);
            image.ReadPixels(new Rect(0, 0, targetTexture.width, targetTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = activeRenderTexture;

            var bytes = image.EncodeToPNG();
            Destroy(image);

            File.WriteAllBytes($"{Application.dataPath}/Resources/Images/{_dropdown.options[_dropdown.value].text}/{_inputField.text}.png", bytes);
        }
    }
}

using Model;

using Parser;

using RotationVisualizer;

using RubikVisualizers;

using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        [SerializeField] private Camera _cameraOllPll;
        [SerializeField] private RubikColorHelper _rubikColorHelper;

        private void Awake() => GetComponent<Button>().onClick.AddListener(PrintCurrentState);

        private void PrintCurrentState()
        {
            if (string.IsNullOrEmpty(_inputField.text) || string.IsNullOrEmpty(_solutionField.text))
            {
                Debug.LogWarning("Empty solution or name cannot save");
                return;
            }
            var dropDownText = _dropdown.options[_dropdown.value].text;
            if (dropDownText == "OLL" || dropDownText == "PLL")
            {
                SaveImageAndCase(_cameraOllPll);
            }
            else
            {
                SaveImageAndCase(_camera);
            }
        }

        private TrainingMode ParseTypeInt() =>
            _dropdown.options[_dropdown.value].text switch
            {
                "F2L" => TrainingMode.F2L,
                "OLL" => TrainingMode.Oll,
                "PLL" => TrainingMode.Pll,
                _ => 0
            };

        private string ParseSolutionText()
        {
            var result = _solutionField.text;
            result = result.Replace("(", string.Empty).Replace(")", string.Empty);
            result = result.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            return result.Replace(' ', ',');
        }

        private void SaveImageAndCase(Camera camera)
        {
            var jsonName = $"{_dropdown.options[_dropdown.value].text}Cases";
            var name = _inputField.text;
            var solutionText = ParseSolutionText();
            var caseType = ParseTypeInt();
            var cases = RubikCaseParser.LoadJson(jsonName);
            var faces = new List<string>();
            foreach (var face in _visualizer.GetFaces)
            {
                var line = new List<RubikColor>(new RubikColor[8]);
                foreach (var faceCube in face.Cubes)
                {
                    var i = face.GetCubeIndex(faceCube.transform.position);
                    line[i] = faceCube.GetComponent<ISetFaceColor>().GetFaceColorForFacing(face.Facing);
                }
                faces.Add(line.Aggregate(string.Empty, (current, color) => current + color));
            }

            var currentCase = cases.FirstOrDefault(x => x.name == name);
            if (currentCase != null)
            {
                cases.Remove(currentCase);
            }
            cases.Add(new RubikCaseParser.RubikCase()
            {
                caseType = caseType,
                faces = faces,
                name = name,
                solution = solutionText,
            });

            RubikCaseParser.SaveJson(jsonName, new(cases));

            var activeRenderTexture = RenderTexture.active;
            RenderTexture.active = camera.targetTexture;

            camera.Render();

            var targetTexture = camera.targetTexture;
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

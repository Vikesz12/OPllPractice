using System;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    [RequireComponent(typeof(TMP_InputField))]
    public class SolutionEditor : MonoBehaviour
    {
        [SerializeField] private Button _saveButton;

        private TMP_InputField _solutionField;

        private void Awake()
        {
            _solutionField = GetComponent<TMP_InputField>();

            _solutionField.onValueChanged.AddListener(CheckSolution);
        }

        private void OnDestroy() => _solutionField.onValueChanged.RemoveListener(CheckSolution);

        private void CheckSolution(string solutionText)
        {
            var turns = solutionText.Split(',');
            foreach (var turn in turns)
            {
                if (turn.Length == 1)
                {
                    if (Enum.TryParse(turn, out BasicRotation basicRot) ||
                        Enum.TryParse(turn, out CubeRotation cubeRot) ||
                        Enum.TryParse(turn, out DoubleLayerRotation doubleLayerRot)) continue;

                    
                }

                if (turn.Length == 2)
                {
                    if (Enum.TryParse(turn[0].ToString(), out BasicRotation basicRot) ||
                        Enum.TryParse(turn[0].ToString(), out CubeRotation cubeRot) ||
                        Enum.TryParse(turn[0].ToString(), out DoubleLayerRotation doubleLayerRot))
                    {
                        if(turn[1] == '2' || turn[1] == '\'') continue;
                    }
                }

                _saveButton.enabled = false;
                return;
            }

            _saveButton.enabled = true;
        }
    }
}

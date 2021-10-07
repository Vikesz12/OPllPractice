using System;
using Model;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace JsonTool
{
    [RequireComponent(typeof(Toggle))]
    public class ColorPicker : MonoBehaviour
    {
        [SerializeField] private RubikColor _colorToSet;
        private void Awake() => GetComponent<Toggle>().onValueChanged.AddListener(OnColorSelected);

        private void OnColorSelected(bool arg0)
        {
            if(!arg0) return;

            JsonToolSettings.CurrentColor = _colorToSet;
            JsonToolSettings.CurrentMaterial = RubikColorMaterialService.GetRubikColorMaterial((RubikColor)_colorToSet);
        }
    }
}

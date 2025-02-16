using EventBus;
using EventBus.Events;

using Model;

using RubikVisualizers;

using Services;

using System.Collections.Generic;

using TMPro;

using UnityEngine;

using Zenject;

public class RubikColorHelper : MonoBehaviour
{
    [Inject] private IEventBus _eventBus;

    [SerializeField] private RubikHolder holder;

    [SerializeField] private List<MeshRenderer> _renderers;

    [SerializeField] private TMP_Dropdown _dropdown;

    private void Awake()
    {
        _eventBus.Subscribe<FaceRotateFinished>(SetColors);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<FaceRotateFinished>(SetColors);
    }

    public void SetColors(FaceRotateFinished _)
    {

        var dropDownText = _dropdown.options[_dropdown.value].text;
        var colors = holder.GetCurrentVisualizer().GetTopColors;
        for (int i = 0; i < colors.Count; i++)
        {
            RubikColor color = colors[i];
            if (dropDownText == "OLL" && color != RubikColor.Y)
            {
                color = RubikColor.L;
            }
            MeshRenderer renderer = _renderers[i];
            renderer.material = RubikColorMaterialService.GetRubikColorMaterial(color);
        }
    }
}

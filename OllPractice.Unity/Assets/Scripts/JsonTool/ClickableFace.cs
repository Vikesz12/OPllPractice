using RubikVisualizers;
using UnityEngine;
using View;

namespace JsonTool
{
    [RequireComponent(typeof(BoxCollider), typeof(MeshRenderer))]
    public class ClickableFace : MonoBehaviour
    {
        private MeshRenderer _renderer;
       
        private void Awake() => _renderer = GetComponent<MeshRenderer>();

        private void OnMouseDown() => _renderer.material = JsonToolSettings.CurrentMaterial;
    }
}

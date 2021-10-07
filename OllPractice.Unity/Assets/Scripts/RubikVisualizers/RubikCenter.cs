using Model;
using Services;
using UnityEngine;

namespace RubikVisualizers
{
    public class RubikCenter : MonoBehaviour, ISetFaceColor
    {
        [SerializeField] private MeshRenderer _faceMeshRenderer;

        public void SetFaceColorForFacing(Vector3 facing, Material materialToSet) => _faceMeshRenderer.material = materialToSet;
        public RubikColor GetFaceColorForFacing(Vector3 facing) => RubikColorMaterialService.GetMaterialColor(_faceMeshRenderer.material);
    }
}

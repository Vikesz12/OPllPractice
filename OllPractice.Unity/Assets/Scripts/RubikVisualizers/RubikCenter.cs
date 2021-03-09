using UnityEngine;

namespace RubikVisualizers
{
    public class RubikCenter : MonoBehaviour, ISetFaceColor
    {
        [SerializeField] private MeshRenderer faceMeshRenderer;

        public void SetFaceColorForFacing(Vector3 facing, Material materialToSet)
        {
            faceMeshRenderer.material = materialToSet;
        }
    }
}

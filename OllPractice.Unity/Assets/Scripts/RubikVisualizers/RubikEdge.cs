using UnityEngine;

namespace RubikVisualizers
{
    public class RubikEdge : MonoBehaviour
    {
        [SerializeField] private MeshRenderer topFaceMeshRenderer;
        [SerializeField] private MeshRenderer frontFaceMeshRenderer;
        public void SetTopFaceColor(Material materialToSet)
        {
            topFaceMeshRenderer.material = materialToSet;
        }

        public void SetFrontFaceColor(Material materialToSet)
        {
            frontFaceMeshRenderer.material = materialToSet;
        }
    }
}

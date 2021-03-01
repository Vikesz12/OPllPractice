using UnityEngine;

namespace RubikVisualizers
{
    public class RubikCorner : MonoBehaviour
    {
        [SerializeField] private MeshRenderer topFaceMeshRenderer;
        [SerializeField] private MeshRenderer leftFaceMeshRenderer;
        [SerializeField] private MeshRenderer frontFaceMeshRenderer;
        public void SetTopFaceColor(Material materialToSet)
        {
            topFaceMeshRenderer.material = materialToSet;
        }

        public void SetLeftFaceColor(Material materialToSet)
        {
            leftFaceMeshRenderer.material = materialToSet;
        }

        public void SetFrontFaceColor(Material materialToSet)
        {
            frontFaceMeshRenderer.material = materialToSet;
        }
    }
}

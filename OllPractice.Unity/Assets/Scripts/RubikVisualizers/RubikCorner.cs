using UnityEngine;

namespace RubikVisualizers
{
    public class RubikCorner : MonoBehaviour, ISetFaceColor
    {
        [SerializeField] private MeshRenderer topFaceMeshRenderer;
        [SerializeField] private MeshRenderer leftFaceMeshRenderer;
        [SerializeField] private MeshRenderer frontFaceMeshRenderer;

        public void SetFaceColorForFacing(Vector3 facing, Material materialToSet)
        {
            if (topFaceMeshRenderer.transform.forward * -1 == facing)
            {
                topFaceMeshRenderer.material = materialToSet;
            }
            else if (leftFaceMeshRenderer.transform.forward * -1 == facing)
            {
                leftFaceMeshRenderer.material = materialToSet;
            }
            else if (frontFaceMeshRenderer.transform.forward * -1 == facing)
            {
                frontFaceMeshRenderer.material = materialToSet;
            }
        }
    }
}

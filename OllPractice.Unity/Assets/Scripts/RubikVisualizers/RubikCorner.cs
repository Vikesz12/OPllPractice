using Extensions;
using UnityEngine;

namespace RubikVisualizers
{
    public class RubikCorner : MonoBehaviour, ISetFaceColor
    {
        [SerializeField] private MeshRenderer _topFaceMeshRenderer;
        [SerializeField] private MeshRenderer _leftFaceMeshRenderer;
        [SerializeField] private MeshRenderer _frontFaceMeshRenderer;

        public void SetFaceColorForFacing(Vector3 facing, Material materialToSet)
        {
            if ((_topFaceMeshRenderer.transform.forward * -1).FuzzyEquals(facing,0.00001f))
            {
                _topFaceMeshRenderer.material = materialToSet;
            }
            else if ((_leftFaceMeshRenderer.transform.forward * -1).FuzzyEquals(facing,0.00001f))
            {
                _leftFaceMeshRenderer.material = materialToSet;
            }
            else if ((_frontFaceMeshRenderer.transform.forward * -1).FuzzyEquals(facing,0.00001f))
            {
                _frontFaceMeshRenderer.material = materialToSet;
            }
        }
    }
}

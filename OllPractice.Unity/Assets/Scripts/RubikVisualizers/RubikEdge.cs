using System;
using UnityEngine;

namespace RubikVisualizers
{
    public class RubikEdge : MonoBehaviour, ISetFaceColor
    {
        [SerializeField] private MeshRenderer topFaceMeshRenderer;
        [SerializeField] private MeshRenderer frontFaceMeshRenderer;
        public void SetFaceColorForFacing(Vector3 facing, Material materialToSet)
        {
            if (topFaceMeshRenderer.transform.up == facing)
            {
                topFaceMeshRenderer.material = materialToSet;
                return;
            }

            if (frontFaceMeshRenderer.transform.up == facing)
            {
                frontFaceMeshRenderer.material = materialToSet;
                return;
            }

            throw new ArgumentOutOfRangeException($"{facing} no matching face found");
        }
    }
}

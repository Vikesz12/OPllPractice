using System;
using UnityEngine;

namespace RubikVisualizers
{
    public class RubikEdge : MonoBehaviour, ISetFaceColor
    {
        [SerializeField] private MeshRenderer _topFaceMeshRenderer;
        [SerializeField] private MeshRenderer _frontFaceMeshRenderer;
        public void SetFaceColorForFacing(Vector3 facing, Material materialToSet)
        {
            if (_topFaceMeshRenderer.transform.up == facing)
            {
                _topFaceMeshRenderer.material = materialToSet;
                return;
            }

            if (_frontFaceMeshRenderer.transform.up == facing)
            {
                _frontFaceMeshRenderer.material = materialToSet;
                return;
            }

            throw new ArgumentOutOfRangeException($"{facing} no matching face found");
        }
    }
}

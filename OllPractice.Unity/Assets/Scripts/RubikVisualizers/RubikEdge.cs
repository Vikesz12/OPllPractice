using Extensions;
using System;
using Model;
using Services;
using UnityEngine;

namespace RubikVisualizers
{
    public class RubikEdge : MonoBehaviour, ISetFaceColor
    {
        [SerializeField] private MeshRenderer _topFaceMeshRenderer;
        [SerializeField] private MeshRenderer _frontFaceMeshRenderer;
        public void SetFaceColorForFacing(Vector3 facing, Material materialToSet)
        {
            if (_topFaceMeshRenderer.transform.up.FuzzyEquals(facing, 0.00001f))
            {
                _topFaceMeshRenderer.material = materialToSet;
                return;
            }

            if (_frontFaceMeshRenderer.transform.up.FuzzyEquals(facing, 0.00001f))
            {
                _frontFaceMeshRenderer.material = materialToSet;
                return;
            }

            throw new ArgumentOutOfRangeException($"{facing} no matching face found on gameojbect {gameObject.name}");
        }

        public RubikColor GetFaceColorForFacing(Vector3 facing)
        {
            if (_topFaceMeshRenderer.transform.up.FuzzyEquals(facing, 0.00001f))
            {
                return RubikColorMaterialService.GetMaterialColor(_topFaceMeshRenderer.material);
            }

            if (_frontFaceMeshRenderer.transform.up.FuzzyEquals(facing, 0.00001f))
            {
                return RubikColorMaterialService.GetMaterialColor(_frontFaceMeshRenderer.material);
            }

            throw new ArgumentOutOfRangeException($"{facing} no matching face found on gameojbect {gameObject.name}");
        }
    }
}

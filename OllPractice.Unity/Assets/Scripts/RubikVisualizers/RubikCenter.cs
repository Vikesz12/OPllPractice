﻿using UnityEngine;

namespace RubikVisualizers
{
    public class RubikCenter : MonoBehaviour
    {
        [SerializeField] private MeshRenderer faceMeshRenderer;
        public void SetFaceColor(Material materialToSet)
        {
            faceMeshRenderer.material = materialToSet;
        }
    }
}

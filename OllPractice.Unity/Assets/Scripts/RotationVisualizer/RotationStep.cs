using Model;
using RubikVisualizers;
using TMPro;
using UnityEngine;

namespace RotationVisualizer
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class RotationStep : MonoBehaviour
    {
        private TextMeshProUGUI _rotationText;

        private FaceRotation _faceRotation;

        private void Awake() => _rotationText = GetComponent<TextMeshProUGUI>();

        public void LoadStep(FaceRotation rotation, Rotation rotationType, bool flipped)
        {
            _rotationText.text = rotation.ToRubikNotation();
            _faceRotation = rotation;
        }

    }
}

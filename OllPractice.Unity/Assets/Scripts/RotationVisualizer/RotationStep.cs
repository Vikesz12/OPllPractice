using System.Collections.Generic;
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
        private bool _secondTurn;
        private Rotation _firstTurnType;
        public bool Finished { get; private set; }

        private void Awake() => _rotationText = GetComponent<TextMeshProUGUI>();

        public void LoadStep(FaceRotation rotation)
        {
            _rotationText.text = rotation.ToRubikNotation();
            _faceRotation = rotation;
        }

        public void SetColor(Color color) => _rotationText.color = color;

        public bool CheckCorrectTurn(FaceRotation rotation, List<FaceRotation> cubeRotations, ref int currentPosition)
        {
            var cubeTurnedRotation = rotation.ToCubeTurnedRotation(cubeRotations);
            if (_faceRotation.RotationType != Rotation.Two)
            {
                if (Equals(cubeTurnedRotation, _faceRotation))
                {
                    _rotationText.color = Color.green;
                    Finished = true;
                    currentPosition++;
                    return true;
                }
                _rotationText.color = Color.red;
                return false;
            }

            switch (_secondTurn)
            {
                case true when Equals(cubeTurnedRotation.BasicRotation, _faceRotation.BasicRotation) 
                               && cubeTurnedRotation.RotationType == _firstTurnType:
                    Finished = true;
                    _rotationText.color = Color.green;
                    currentPosition++;
                    return true;
                case true:
                    _rotationText.color = Color.black;
                    return false;
            }

            if (Equals(cubeTurnedRotation.BasicRotation, _faceRotation.BasicRotation))
            {
                _rotationText.color = Color.yellow;
                _secondTurn = true;
                _firstTurnType = cubeTurnedRotation.RotationType;
                return true;
            }

            _rotationText.color = Color.red;
            return false;
        }
    }
}

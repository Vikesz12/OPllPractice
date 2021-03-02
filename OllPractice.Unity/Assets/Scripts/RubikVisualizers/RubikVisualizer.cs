using Model;
using Parser;
using Scanner;
using Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using View;

namespace RubikVisualizers
{
    public class RubikVisualizer : MonoBehaviour
    {
        private NotificationParser _notificationParse;
        private Cube _cube;
        private RubikScanner _rubikScanner;
        private List<FaceView> _faces;
        private Queue<KeyValuePair<FaceView, IEnumerator>> _animations;
        private bool _isAnimating;
        private KeyValuePair<FaceView, IEnumerator> _currentAnimation;

        public void Start()
        {
            _notificationParse = new NotificationParser();
            _rubikScanner = GetComponent<RubikScanner>();
            _cube = new Cube();
            _animations = new Queue<KeyValuePair<FaceView, IEnumerator>>();
            _faces = GetComponentsInChildren<FaceView>().ToList();
            for (var i = 0; i < 6; i++)
            {
                var center = transform.GetChild(i).gameObject;
                center.GetComponent<RubikCenter>()
                    .SetFaceColor(RubikColorMaterialService.GetRubikColorMaterial((RubikColor)i));
            }

            var faces = new Face[6];
            for (var i = 0; i < 6; i++)
            {
                var rubikColors = Enumerable.Repeat((RubikColor)i, 8);
                faces[i] = new Face(rubikColors);
            }
            LoadState(faces);

        }

        public void LoadState(Face[] faces)
        {
            //top edges
            for (var i = 0; i < 4; i++)
            {
                const int startIndex = 6;
                var whiteEdge = transform.GetChild(startIndex + i).gameObject;
                var rubikEdge = whiteEdge.GetComponent<RubikEdge>();
                var topIndex = 0;
                switch (i)
                {
                    case 0:
                        topIndex = 5;
                        break;
                    case 1:
                        topIndex = 7;
                        break;
                    case 2:
                        topIndex = 3;
                        break;
                    case 3:
                        topIndex = 1;
                        break;

                }
                rubikEdge.SetTopFaceColor(RubikColorMaterialService.GetRubikColorMaterial(faces[0].GetColorAt(topIndex)));
                rubikEdge.SetFrontFaceColor(RubikColorMaterialService.GetRubikColorMaterial(faces[i + 1].GetColorAt(1)));
                _faces[0].AddCube(whiteEdge);
                _faces[i + 1].AddCube(whiteEdge);
            }

            //middle edges
            for (var i = 0; i < 4; i++)
            {
                const int startIndex = 10;
                var middleEdge = transform.GetChild(startIndex + i).gameObject;
                var rubikEdge = middleEdge.GetComponent<RubikEdge>();
                _faces[i + 1].AddCube(middleEdge);
                switch (i)
                {
                    case 0:
                        _faces[3].AddCube(middleEdge);
                        rubikEdge.SetTopFaceColor(RubikColorMaterialService.GetRubikColorMaterial(faces[1].GetColorAt(3)));
                        rubikEdge.SetFrontFaceColor(RubikColorMaterialService.GetRubikColorMaterial(faces[3].GetColorAt(7)));
                        break;

                    case 1:
                        _faces[1].AddCube(middleEdge);
                        rubikEdge.SetTopFaceColor(RubikColorMaterialService.GetRubikColorMaterial(faces[1].GetColorAt(7)));
                        rubikEdge.SetFrontFaceColor(RubikColorMaterialService.GetRubikColorMaterial(faces[2].GetColorAt(3)));
                        break;

                    case 2:
                        _faces[4].AddCube(middleEdge);
                        rubikEdge.SetTopFaceColor(RubikColorMaterialService.GetRubikColorMaterial(faces[4].GetColorAt(7)));
                        rubikEdge.SetFrontFaceColor(RubikColorMaterialService.GetRubikColorMaterial(faces[3].GetColorAt(3)));
                        break;

                    case 3:
                        _faces[2].AddCube(middleEdge);
                        rubikEdge.SetTopFaceColor(RubikColorMaterialService.GetRubikColorMaterial(faces[4].GetColorAt(3)));
                        rubikEdge.SetFrontFaceColor(RubikColorMaterialService.GetRubikColorMaterial(faces[2].GetColorAt(7)));
                        break;
                }
            }

            //bottom edges
            for (var i = 0; i < 4; i++)
            {
                const int startIndex = 14;
                var yellowEdge = transform.GetChild(startIndex + i).gameObject;
                var rubikEdge = yellowEdge.GetComponent<RubikEdge>();
                var topIndex = 0;
                switch (i)
                {
                    case 0:
                        topIndex = 5;
                        break;
                    case 1:
                        topIndex = 3;
                        break;
                    case 2:
                        topIndex = 7;
                        break;
                    case 3:
                        topIndex = 1;
                        break;

                }
                rubikEdge.SetTopFaceColor(RubikColorMaterialService.GetRubikColorMaterial(faces[5].GetColorAt(topIndex)));
                rubikEdge.SetFrontFaceColor(RubikColorMaterialService.GetRubikColorMaterial(faces[1 + i].GetColorAt(5)));
                _faces[5].AddCube(yellowEdge);
                _faces[i + 1].AddCube(yellowEdge);

            }

            //top corners
            for (var i = 0; i < 4; i++)
            {
                const int startIndex = 18;
                var topCorner = transform.GetChild(startIndex + i).gameObject;
                var rubikEdge = topCorner.GetComponent<RubikCorner>();
                rubikEdge.SetLeftFaceColor(RubikColorMaterialService.GetRubikColorMaterial(faces[i + 1].GetColorAt(0)));
                switch (i)
                {
                    case 0:
                        rubikEdge.SetTopFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[0].GetColorAt(6)));
                        rubikEdge.SetFrontFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[2].GetColorAt(2)));
                        _faces[2].AddCube(topCorner);
                        _faces[1].AddCube(topCorner);
                        break;
                    case 1:
                        rubikEdge.SetTopFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[0].GetColorAt(0)));
                        rubikEdge.SetFrontFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[4].GetColorAt(2)));
                        _faces[4].AddCube(topCorner);
                        _faces[2].AddCube(topCorner);
                        break;
                    case 2:
                        rubikEdge.SetTopFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[0].GetColorAt(4)));
                        rubikEdge.SetFrontFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[1].GetColorAt(2)));
                        _faces[1].AddCube(topCorner);
                        _faces[3].AddCube(topCorner);
                        break;
                    case 3:
                        rubikEdge.SetTopFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[0].GetColorAt(2)));
                        rubikEdge.SetFrontFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[3].GetColorAt(2)));
                        _faces[3].AddCube(topCorner);
                        _faces[4].AddCube(topCorner);
                        break;
                }

                _faces[0].AddCube(topCorner);
            }

            //bottom corners
            for (var i = 0; i < 4; i++)
            {
                const int startIndex = 22;
                var bottomCorner = transform.GetChild(startIndex + i).gameObject;

                var rubikEdge = bottomCorner.GetComponent<RubikCorner>();
                rubikEdge.SetLeftFaceColor(RubikColorMaterialService.GetRubikColorMaterial(faces[i + 1].GetColorAt(4)));
                switch (i)
                {
                    case 0:
                        rubikEdge.SetTopFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[5].GetColorAt(6)));
                        rubikEdge.SetFrontFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[3].GetColorAt(6)));
                        _faces[3].AddCube(bottomCorner);
                        _faces[1].AddCube(bottomCorner);
                        break;
                    case 1:
                        rubikEdge.SetTopFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[5].GetColorAt(4)));
                        rubikEdge.SetFrontFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[1].GetColorAt(6)));
                        _faces[1].AddCube(bottomCorner);
                        _faces[2].AddCube(bottomCorner);
                        break;
                    case 2:
                        rubikEdge.SetTopFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[5].GetColorAt(0)));
                        rubikEdge.SetFrontFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[4].GetColorAt(6)));
                        _faces[4].AddCube(bottomCorner);
                        _faces[3].AddCube(bottomCorner);
                        break;
                    case 3:
                        rubikEdge.SetTopFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[5].GetColorAt(2)));
                        rubikEdge.SetFrontFaceColor(
                            RubikColorMaterialService.GetRubikColorMaterial(faces[2].GetColorAt(6)));
                        _faces[4].AddCube(bottomCorner);
                        _faces[2].AddCube(bottomCorner);
                        break;
                }

                _faces[5].AddCube(bottomCorner);
            }
        }


        public void ProcessMessage(byte[] notification, short dataSize) => _notificationParse.ParseNotification(notification, dataSize, _cube, this);

        public void U() => RotateSide(0, 1, 2, 4, 3, Rotation.ONE);

        public void UPrime() => RotateSide(0, 1, 3, 4, 2, Rotation.PRIME);

        public void R() => RotateSide(3, 0, 4, 5, 1, Rotation.ONE);

        public void RPrime() => RotateSide(3, 0, 1, 5, 4, Rotation.PRIME);


        public void L() => RotateSide(2, 0, 1, 5, 4, Rotation.ONE);

        public void LPrime() => RotateSide(2, 0, 4, 5, 1, Rotation.PRIME);

        public void F() => RotateSide(1, 0, 3, 5, 2, Rotation.ONE);

        public void FPrime() => RotateSide(1, 0, 2, 5, 3, Rotation.PRIME);

        public void B() => RotateSide(4, 0, 2, 5, 3, Rotation.ONE);

        public void BPrime() => RotateSide(4, 0, 3, 5, 2, Rotation.PRIME);

        public void D() => RotateSide(5, 1, 3, 4, 2, Rotation.ONE);

        public void DPrime() => RotateSide(5, 1, 2, 4, 3, Rotation.PRIME);

        private void RotateSide(int sideToRotate, int side1, int side2, int side3, int side4, Rotation rotation)
        {
            _animations.Enqueue(
                new KeyValuePair<FaceView, IEnumerator>(
                    _faces[sideToRotate],
                    _faces[sideToRotate].RotateCoroutine(rotation, this, () =>
                    {

                        var firstSideCubes = _faces[side1].RemoveCubes(_faces[sideToRotate].Cubes);
                        var secondSideCubes = _faces[side2].RemoveCubes(_faces[sideToRotate].Cubes);
                        var thirdSideCubes = _faces[side3].RemoveCubes(_faces[sideToRotate].Cubes);
                        var redSideCubes = _faces[side4].RemoveCubes(_faces[sideToRotate].Cubes);

                        _faces[side2].AddCubes(firstSideCubes);
                        _faces[side3].AddCubes(secondSideCubes);
                        _faces[side4].AddCubes(thirdSideCubes);
                        _faces[side1].AddCubes(redSideCubes);

                        _isAnimating = false;
                    })));

        }

        public void Update()
        {
            if (_animations.Count == 0) return;

            if (_animations.Count > 3)
            {
                if (_isAnimating)
                {
                    StopCoroutine(_currentAnimation.Value);
                    _isAnimating = false;
                }
                else
                {
                    _currentAnimation = _animations.Dequeue();
                }
                _currentAnimation.Key.SkipCoroutine(this);
            }
            else if (!_isAnimating)
            {
                _currentAnimation = _animations.Dequeue();
                StartCoroutine(_currentAnimation.Value);
                _isAnimating = true;
            }
        }
    }
}

using Model;
using Parser;
using Scanner;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using View;

namespace RubikVisualizers
{
    public class RubikVisualizer : MonoBehaviour
    {
        private Cube _cube;
        private List<FaceView> _faces;
        private Queue<KeyValuePair<Action<bool>, IEnumerator>> _animations;
        private bool _isAnimating;
        private KeyValuePair<Action<bool>, IEnumerator> _currentAnimation;
        private bool _setupComplete;

        public void Awake()
        {
            _cube = new Cube();
            _animations = new Queue<KeyValuePair<Action<bool>, IEnumerator>>();
            _faces = GetComponentsInChildren<FaceView>().ToList();
            for (var i = 0; i < 6; i++)
            {
                var center = transform.GetChild(i).gameObject;
                center.GetComponent<RubikCenter>()
                    .SetFaceColorForFacing(center.transform.up, RubikColorMaterialService.GetRubikColorMaterial((RubikColor)i));
            }
        }

        public void Start()
        {
            SetupFaces();
        }

        private void SetupFaces()
        {
            if (_setupComplete) return;
            //top edges
            for (var i = 0; i < 4; i++)
            {
                const int startIndex = 6;
                var whiteEdge = transform.GetChild(startIndex + i).gameObject;
                _faces[0].AddCube(whiteEdge);
                _faces[i + 1].AddCube(whiteEdge);
            }

            //middle edges
            for (var i = 0; i < 4; i++)
            {
                const int startIndex = 10;
                var middleEdge = transform.GetChild(startIndex + i).gameObject;
                _faces[i + 1].AddCube(middleEdge);
                switch (i)
                {
                    case 0:
                        _faces[3].AddCube(middleEdge);
                        break;

                    case 1:
                        _faces[1].AddCube(middleEdge);
                        break;

                    case 2:
                        _faces[4].AddCube(middleEdge);
                        break;

                    case 3:
                        _faces[2].AddCube(middleEdge);
                        break;
                }
            }

            //bottom edges
            for (var i = 0; i < 4; i++)
            {
                const int startIndex = 14;
                var yellowEdge = transform.GetChild(startIndex + i).gameObject;
                _faces[5].AddCube(yellowEdge);
                _faces[i + 1].AddCube(yellowEdge);
            }

            //top corners
            for (var i = 0; i < 4; i++)
            {
                const int startIndex = 18;
                var topCorner = transform.GetChild(startIndex + i).gameObject;
                switch (i)
                {
                    case 0:
                        _faces[2].AddCube(topCorner);
                        _faces[1].AddCube(topCorner);
                        break;
                    case 1:
                        _faces[4].AddCube(topCorner);
                        _faces[2].AddCube(topCorner);
                        break;
                    case 2:
                        _faces[1].AddCube(topCorner);
                        _faces[3].AddCube(topCorner);
                        break;
                    case 3:
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

                switch (i)
                {
                    case 0:
                        _faces[3].AddCube(bottomCorner);
                        _faces[1].AddCube(bottomCorner);
                        break;
                    case 1:
                        _faces[1].AddCube(bottomCorner);
                        _faces[2].AddCube(bottomCorner);
                        break;
                    case 2:
                        _faces[4].AddCube(bottomCorner);
                        _faces[3].AddCube(bottomCorner);
                        break;
                    case 3:
                        _faces[4].AddCube(bottomCorner);
                        _faces[2].AddCube(bottomCorner);
                        break;
                }

                _faces[5].AddCube(bottomCorner);
            }

            _setupComplete = true;
        }


        public void NotificationParserOnFaceRotated(FaceRotation rotation)
        {

            switch (rotation)
            {
                case FaceRotation.R:
                    _cube.R();
                    R();
                    break;
                case FaceRotation.RPrime:
                    _cube.RPrime();
                    RPrime();
                    break;
                case FaceRotation.U:
                    _cube.U();
                    U();
                    break;
                case FaceRotation.UPrime:
                    _cube.UPrime();
                    UPrime();
                    break;
                case FaceRotation.L:
                    _cube.L();
                    L();
                    break;
                case FaceRotation.LPrime:
                    _cube.LPrime();
                    LPrime();
                    break;
                case FaceRotation.F:
                    _cube.F();
                    F();
                    break;
                case FaceRotation.FPrime:
                    _cube.FPrime();
                    FPrime();
                    break;
                case FaceRotation.B:
                    _cube.B();
                    B();
                    break;
                case FaceRotation.BPrime:
                    _cube.BPrime();
                    BPrime();
                    break;
                case FaceRotation.D:
                    _cube.D();
                    D();
                    break;
                case FaceRotation.DPrime:
                    _cube.DPrime();
                    DPrime();
                    break;
                case FaceRotation.M:
                    _cube.R();
                    _cube.LPrime();
                    R();
                    LPrime();
                    break;
                case FaceRotation.MPrime:
                    _cube.RPrime();
                    _cube.L();
                    RPrime();
                    L();
                    break;
                case FaceRotation.M2:
                    _cube.FPrime();
                    _cube.B();
                    FPrime();
                    B();
                    break;
                case FaceRotation.M2Prime:
                    _cube.F();
                    _cube.BPrime();
                    F();
                    BPrime();
                    break;
                case FaceRotation.M3:
                    _cube.DPrime();
                    _cube.U();
                    DPrime();
                    U();
                    break;
                case FaceRotation.M3Prime:
                    _cube.D();
                    _cube.UPrime();
                    D();
                    UPrime();
                    break;
                case FaceRotation.Y:
                    Y();
                    break;
                case FaceRotation.YPrime:
                    YPrime();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
            }
        }

        public void LoadState(Face[] faces)
        {
            SetupFaces();
            _cube.LoadState(faces);
            for (var i = 0; i < _faces.Count; i++)
            {
                var face = _faces[i];
                foreach (var cube in face.Cubes)
                {
                    var cubeIndex = face.GetCubeIndex(cube.transform.position);
                    var rubikColor = faces[i].GetColorAt(cubeIndex);
                    cube.GetComponent<ISetFaceColor>().SetFaceColorForFacing(face.Facing, RubikColorMaterialService.GetRubikColorMaterial(rubikColor));
                }
            }
        }



        private void U() => RotateSide(0, 1, 2, 4, 3, Rotation.ONE);

        private void UPrime() => RotateSide(0, 1, 3, 4, 2, Rotation.PRIME);

        private void R() => RotateSide(3, 0, 4, 5, 1, Rotation.ONE);

        private void RPrime() => RotateSide(3, 0, 1, 5, 4, Rotation.PRIME);


        private void L() => RotateSide(2, 0, 1, 5, 4, Rotation.ONE);

        private void LPrime() => RotateSide(2, 0, 4, 5, 1, Rotation.PRIME);

        private void F() => RotateSide(1, 0, 3, 5, 2, Rotation.ONE);

        private void FPrime() => RotateSide(1, 0, 2, 5, 3, Rotation.PRIME);

        private void B() => RotateSide(4, 0, 2, 5, 3, Rotation.ONE);

        private void BPrime() => RotateSide(4, 0, 3, 5, 2, Rotation.PRIME);

        private void D() => RotateSide(5, 1, 3, 4, 2, Rotation.ONE);

        private void DPrime() => RotateSide(5, 1, 2, 4, 3, Rotation.PRIME);

        private void RotateSide(int sideToRotate, int side1, int side2, int side3, int side4, Rotation rotation)
        {
            _animations.Enqueue(
                new KeyValuePair<Action<bool>, IEnumerator>(
                    skip =>
                    {
                        if (skip)
                            _faces[sideToRotate].SkipCoroutine();

                        var firstSideCubes = _faces[side1].RemoveCubes(_faces[sideToRotate].Cubes);
                        var secondSideCubes = _faces[side2].RemoveCubes(_faces[sideToRotate].Cubes);
                        var thirdSideCubes = _faces[side3].RemoveCubes(_faces[sideToRotate].Cubes);
                        var redSideCubes = _faces[side4].RemoveCubes(_faces[sideToRotate].Cubes);

                        _faces[side2].AddCubes(firstSideCubes);
                        _faces[side3].AddCubes(secondSideCubes);
                        _faces[side4].AddCubes(thirdSideCubes);
                        _faces[side1].AddCubes(redSideCubes);

                        if (!skip)
                            _faces[sideToRotate].RotateWithoutAnimation(rotation);
                    },
                    _faces[sideToRotate].RotateCoroutine(rotation, () =>
                    {

                        _faces[sideToRotate].SkipCoroutine();
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

        public void Y()
        {
            RotateCube(FaceRotation.Y);
        }

        public void YPrime()
        {
            RotateCube(FaceRotation.YPrime);
        }

        private void RotateCube(FaceRotation rotation)
        {
            _animations.Enqueue(new KeyValuePair<Action<bool>, IEnumerator>(
                skip =>
                {
                    if(!skip)
                        CubeRotateWithoutAnimation(rotation);
                },
                CubeRotationCoroutine(rotation, () => _isAnimating = false)
            ));
        }

        private IEnumerator CubeRotationCoroutine(FaceRotation rotation, Action callback)
        {
            var currentTime = 0.0f;
            var angleToRotate = rotation == FaceRotation.Y ? -90f : 90f;
            const float timeToRotate = 0.25f;
            while (currentTime <= timeToRotate)
            {
                currentTime += Time.deltaTime; 
                gameObject.transform.RotateAround(gameObject.transform.position, gameObject.transform.up, angleToRotate * (Time.deltaTime / timeToRotate));
                yield return null;
            }
            callback.Invoke();
        }

        private void CubeRotateWithoutAnimation(FaceRotation rotation)
        {
            var angleToRotate = rotation == FaceRotation.Y ? 90f : -90f;
            gameObject.transform.RotateAround(gameObject.transform.position, gameObject.transform.up, angleToRotate);
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
                    _currentAnimation.Key.Invoke(true);
                }
                else
                {
                    _currentAnimation = _animations.Dequeue();
                    _currentAnimation.Key.Invoke(false);
                }
            }
            else if (!_isAnimating)
            {
                _currentAnimation = _animations.Dequeue();
                StartCoroutine(_currentAnimation.Value);
                _isAnimating = true;
            }
        }

        public void Flip()
        {
            var transform1 = transform;
            if (transform.localPosition.y == 0f)
            {
                transform1.localPosition = new Vector3(0, -0.75f, 0);
                transform1.localEulerAngles = new Vector3(0, -75f, 180f);
            }
            else
            {
                transform1.localPosition = Vector3.zero;
                transform1.localEulerAngles = Vector3.zero;
            }
        }
    }
}

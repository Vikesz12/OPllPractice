using EventBus;
using EventBus.Events;
using Model;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using View;
using Zenject;

namespace RubikVisualizers
{
    public class RubikVisualizer : MonoBehaviour
    {
        [Inject] private IEventBus _eventBus;

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

            _eventBus.Subscribe<FaceRotated>(OnFaceRotated);
            _eventBus.Subscribe<StateParsed>(parsed => LoadState(parsed.Faces));
        }

        public void Start() => SetupFaces();

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<FaceRotated>(OnFaceRotated);
            _eventBus.Unsubscribe<StateParsed>(parsed => LoadState(parsed.Faces));
        }

        public Cube GetCurrentCube => _cube;

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
                        _faces[2].AddCube(middleEdge);
                        break;

                    case 1:
                        _faces[3].AddCube(middleEdge);
                        break;

                    case 2:
                        _faces[4].AddCube(middleEdge);
                        break;

                    case 3:
                        _faces[1].AddCube(middleEdge);
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
                        _faces[3].AddCube(topCorner);
                        _faces[2].AddCube(topCorner);
                        break;
                    case 1:
                        _faces[4].AddCube(topCorner);
                        _faces[3].AddCube(topCorner);
                        break;
                    case 2:
                        _faces[2].AddCube(topCorner);
                        _faces[1].AddCube(topCorner);
                        break;
                    case 3:
                        _faces[1].AddCube(topCorner);
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
                        _faces[1].AddCube(bottomCorner);
                        _faces[2].AddCube(bottomCorner);
                        break;
                    case 1:
                        _faces[2].AddCube(bottomCorner);
                        _faces[3].AddCube(bottomCorner);
                        break;
                    case 2:
                        _faces[4].AddCube(bottomCorner);
                        _faces[1].AddCube(bottomCorner);
                        break;
                    case 3:
                        _faces[4].AddCube(bottomCorner);
                        _faces[3].AddCube(bottomCorner);
                        break;
                }

                _faces[5].AddCube(bottomCorner);
            }

            _setupComplete = true;
        }


        private void OnFaceRotated(FaceRotated faceRotated)
        {
            var rotation = faceRotated.Rotation;

            if (!rotation.IsCubeRotation)
            {
                switch (rotation.BasicRotation)
                {
                    case BasicRotation.R:
                        if (rotation.RotationType == Rotation.One)
                        {
                            R();
                        }
                        else if (rotation.RotationType == Rotation.Prime)
                        {
                            RPrime();
                        }

                        break;
                    case BasicRotation.U:
                        if (rotation.RotationType == Rotation.One)
                        {
                            U();
                        }
                        else if (rotation.RotationType == Rotation.Prime)
                        {
                            UPrime();
                        }

                        break;
                    case BasicRotation.L:
                        if (rotation.RotationType == Rotation.One)
                        {
                            L();
                        }
                        else if (rotation.RotationType == Rotation.Prime)
                        {
                            LPrime();
                        }

                        break;
                    case BasicRotation.F:
                        if (rotation.RotationType == Rotation.One)
                        {
                            F();
                        }
                        else if (rotation.RotationType == Rotation.Prime)
                        {
                            FPrime();
                        }

                        break;
                    case BasicRotation.B:
                        if (rotation.RotationType == Rotation.One)
                        {
                            B();
                        }
                        else if (rotation.RotationType == Rotation.Prime)
                        {
                            BPrime();
                        }

                        break;
                    case BasicRotation.D:
                        if (rotation.RotationType == Rotation.One)
                        {
                            D();
                        }
                        else if (rotation.RotationType == Rotation.Prime)
                        {
                            DPrime();
                        }

                        break;
                    case BasicRotation.M:
                        if (rotation.RotationType == Rotation.One)
                        {
                            R();
                            LPrime();
                        }
                        else if (rotation.RotationType == Rotation.Prime)
                        {
                            RPrime();
                            L();
                        }

                        break;
                    case BasicRotation.M2:
                        if (rotation.RotationType == Rotation.One)
                        {
                            FPrime();
                            B();
                        }
                        else if (rotation.RotationType == Rotation.Prime)
                        {
                            F();
                            BPrime();
                        }

                        break;
                    case BasicRotation.M3:
                        if (rotation.RotationType == Rotation.One)
                        {
                            DPrime();
                            U();
                        }
                        else if (rotation.RotationType == Rotation.Prime)
                        {
                            D();
                            UPrime();
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(rotation.BasicRotation), rotation.BasicRotation,
                            null);
                }
            }

            else
            {
                switch (rotation.CubeRotation)
                {
                    case CubeRotation.Y:
                        if (rotation.RotationType == Rotation.One)
                        {
                            Y();
                        }
                        else if (rotation.RotationType == Rotation.Prime)
                        {
                            YPrime();
                        }
                        break;
                    case CubeRotation.X:
                        if (rotation.RotationType == Rotation.One)
                        {
                            X();
                        }
                        else if (rotation.RotationType == Rotation.Prime)
                        {
                            XPrime();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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



        private void U()
        {
            _cube.U();
            RotateSide(0, 2, 3, 4, 1, Rotation.One);
        }

        private void UPrime()
        {
            _cube.UPrime();
            RotateSide(0, 2, 1, 4, 3, Rotation.Prime);
        }

        private void R()
        {
            _cube.R();
            RotateSide(1, 0, 4, 5, 2, Rotation.One);
        }

        private void RPrime()
        {
            _cube.RPrime();
            RotateSide(1, 0, 2, 5, 4, Rotation.Prime);
        }


        private void L()
        {
            _cube.L();
            RotateSide(3, 0, 2, 5, 4, Rotation.One);
        }

        private void LPrime()
        {
            _cube.LPrime();
            RotateSide(3, 0, 4, 5, 2, Rotation.Prime);
        }

        private void F()
        {
            _cube.F();
            RotateSide(2, 0, 1, 5, 3, Rotation.One);
        }

        private void FPrime()
        {
            _cube.FPrime();
            RotateSide(2, 0, 3, 5, 1, Rotation.Prime);
        }

        private void B()
        {
            _cube.B();
            RotateSide(4, 0, 3, 5, 1, Rotation.One);
        }

        private void BPrime()
        {
            _cube.BPrime();
            RotateSide(4, 0, 1, 5, 3, Rotation.Prime);
        }

        private void D()
        {
            _cube.D();
            RotateSide(5, 2, 1, 4, 3, Rotation.One);
        }

        private void DPrime()
        {
            _cube.DPrime();
            RotateSide(5, 2, 3, 4, 1, Rotation.Prime);
        }

        private void RotateSide(int sideToRotate, int side1, int side2, int side3, int side4, Rotation rotation) =>
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

        public void Y() => RotateCube(new FaceRotation(CubeRotation.Y, Rotation.One));

        public void YPrime() => RotateCube(new FaceRotation(CubeRotation.Y, Rotation.Prime));

        public void X() => RotateCube(new FaceRotation(CubeRotation.X, Rotation.One));
        public void XPrime() => RotateCube(new FaceRotation(CubeRotation.X, Rotation.Prime));

        private void RotateCube(FaceRotation rotation) =>
            _animations.Enqueue(new KeyValuePair<Action<bool>, IEnumerator>(
                skip =>
                {
                    if (!skip)
                        CubeRotateWithoutAnimation(rotation);
                },
                CubeRotationCoroutine(rotation, () => _isAnimating = false)
            ));

        private IEnumerator CubeRotationCoroutine(FaceRotation rotation, Action callback)
        {
            var currentTime = 0.0f;

            var angleToRotate = rotation.RotationType switch
            {
                Rotation.Prime => -90f,
                Rotation.One => 90f,
                Rotation.Two => 180f,
                _ => throw new ArgumentOutOfRangeException()
            };

            var o = gameObject;
            var axisToRotateAround = rotation.CubeRotation switch
            {
                CubeRotation.Y => o.transform.up,
                CubeRotation.X => o.transform.right,
                _ => throw new ArgumentOutOfRangeException()
            };

            const float timeToRotate = 0.25f;
            while (currentTime <= timeToRotate)
            {
                currentTime += Time.deltaTime;
                gameObject.transform.RotateAround(o.transform.position, axisToRotateAround, angleToRotate * (Time.deltaTime / timeToRotate));
                yield return null;
            }
            callback.Invoke();
        }

        private void CubeRotateWithoutAnimation(FaceRotation rotation)
        {
            var angleToRotate = rotation.RotationType switch
            {
                Rotation.Prime => -90f,
                Rotation.One => 90f,
                Rotation.Two => 180f,
                _ => throw new ArgumentOutOfRangeException()
            };
            var o = gameObject;

            var axisToRotateAround = rotation.CubeRotation switch
            {
                CubeRotation.Y => o.transform.up,
                CubeRotation.X => o.transform.right,
                _ => throw new ArgumentOutOfRangeException()
            };

            gameObject.transform.RotateAround(o.transform.position, axisToRotateAround, angleToRotate);
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
            if (transform.localEulerAngles == Vector3.zero)
            {
                transform1.localPosition = new Vector3(0, -0.45f, 0);
                transform1.localEulerAngles = new Vector3(0, -75f, 180f);
            }
            else
            {
                transform1.localPosition = new Vector3(0, -0.12f, 0);
                transform1.localEulerAngles = Vector3.zero;
            }
        }
    }
}

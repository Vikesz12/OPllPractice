using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class Cube
    {
        private Face[] _faces;

        public Cube()
        {
            _faces = new Face[6];
            var centers = new RubikColor[6];
            //Init solved cube
            //Top face
            _faces[0] = new Face(RubikColor.W);
            centers[0] = RubikColor.W;
            //Right Face
            _faces[1] = new Face(RubikColor.R);
            centers[1] = RubikColor.R;
            //Front face
            _faces[2] = new Face(RubikColor.G);
            centers[2] = RubikColor.G;
            //Left face
            _faces[3] = new Face(RubikColor.O);
            centers[3] = RubikColor.O;
            //Back face
            _faces[4] = new Face(RubikColor.B);
            centers[4] = RubikColor.B;
            //Bottom face
            _faces[5] = new Face(RubikColor.Y);
            centers[5] = RubikColor.Y;
        }

        public Face[] GetFaces => _faces;

        public void U()
        {
            _faces[0].Rotate(Rotation.One);
            var greenSideBytes = _faces[2].RotateSide(0);
            var orangeSideBytes = _faces[3].RotateSide(0, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(0, orangeSideBytes);
            var redSideBytes = _faces[1].RotateSide(0, blueSideBytes);
            _faces[2].RotateSide(0, redSideBytes);
        }

        public void UPrime()
        {
            _faces[0].Rotate(Rotation.Prime);
            var greenSideBytes = _faces[2].RotateSide(0);
            var redSideBytes = _faces[1].RotateSide(0, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(0, redSideBytes);
            var orangeSideBytes = _faces[3].RotateSide(0, blueSideBytes);
            _faces[2].RotateSide(0, orangeSideBytes);
        }

        public void R()
        {
            _faces[1].Rotate(Rotation.One);
            var greenSideBytes = _faces[2].RotateSide(2);
            var whiteSideBytes = _faces[0].RotateSide(2, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(6, whiteSideBytes);
            var yellowSideBytes = _faces[5].RotateSide(2, blueSideBytes);
            _faces[2].RotateSide(2, yellowSideBytes);
        }
        public void RPrime()
        {
            _faces[1].Rotate(Rotation.Prime);
            var greenSideBytes = _faces[2].RotateSide(2);
            var yellowSideBytes = _faces[5].RotateSide(2, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(6, yellowSideBytes);
            var whiteSideBytes = _faces[0].RotateSide(2, blueSideBytes);
            _faces[2].RotateSide(2, whiteSideBytes);
        }
        public void L()
        {
            _faces[3].Rotate(Rotation.One);
            var greenSideBytes = _faces[2].RotateSide(6);
            var yellowSideBytes = _faces[5].RotateSide(6, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(2, yellowSideBytes);
            var whiteSideBytes = _faces[0].RotateSide(6, blueSideBytes);
            _faces[2].RotateSide(6, whiteSideBytes);

        }
        public void LPrime()
        {
            _faces[3].Rotate(Rotation.Prime);
            var greenSideBytes = _faces[2].RotateSide(6);
            var whiteSideBytes = _faces[0].RotateSide(6, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(2, whiteSideBytes);
            var yellowSideBytes = _faces[5].RotateSide(6, blueSideBytes);
            _faces[2].RotateSide(6, yellowSideBytes);

        }
        public void F()
        {
            _faces[2].Rotate(Rotation.One);
            var whiteSideBytes = _faces[0].RotateSide(4);
            var redSideBytes = _faces[1].RotateSide(6, whiteSideBytes);
            var yellowSideBytes = _faces[5].RotateSide(0, redSideBytes);
            var orangeSideBytes = _faces[3].RotateSide(2, yellowSideBytes);
            _faces[0].RotateSide(4, orangeSideBytes);

        }
        public void FPrime()
        {
            _faces[2].Rotate(Rotation.Prime);
            var whiteSideBytes = _faces[0].RotateSide(4);
            var orangeSideBytes = _faces[3].RotateSide(2, whiteSideBytes);
            var yellowSideBytes = _faces[5].RotateSide(0, orangeSideBytes);
            var redSideBytes = _faces[1].RotateSide(6, yellowSideBytes);
            _faces[0].RotateSide(4, redSideBytes);

        }
        public void D()
        {
            _faces[5].Rotate(Rotation.One);
            var greenSideBytes = _faces[2].RotateSide(4);
            var redSideBytes = _faces[1].RotateSide(4, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(4, redSideBytes);
            var orangeSideBytes = _faces[3].RotateSide(4, blueSideBytes);
            _faces[2].RotateSide(4, orangeSideBytes);


        }
        public void DPrime()
        {
            _faces[5].Rotate(Rotation.Prime);
            var greenSideBytes = _faces[2].RotateSide(4);
            var orangeSideBytes = _faces[3].RotateSide(4, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(4, orangeSideBytes);
            var redSideBytes = _faces[1].RotateSide(4, blueSideBytes);
            _faces[2].RotateSide(4, redSideBytes);

        }
        public void B()
        {
            _faces[4].Rotate(Rotation.One);
            var whiteSideBytes = _faces[0].RotateSide(0);
            var orangeSideBytes = _faces[3].RotateSide(6, whiteSideBytes);
            var yellowSideBytes = _faces[5].RotateSide(4, orangeSideBytes);
            var redSideBytes = _faces[1].RotateSide(2, yellowSideBytes);
            _faces[0].RotateSide(0, redSideBytes);


        }
        public void BPrime()
        {
            _faces[4].Rotate(Rotation.Prime);
            var whiteSideBytes = _faces[0].RotateSide(0);
            var redSideBytes = _faces[1].RotateSide(2, whiteSideBytes);
            var yellowSideBytes = _faces[5].RotateSide(4, redSideBytes);
            var orangeSideBytes = _faces[3].RotateSide(6, yellowSideBytes);
            _faces[0].RotateSide(0, orangeSideBytes);

        }

        public void DoAlgorithm(List<FaceRotation> cubeRotations, List<FaceRotation> rotations)
        {
            SearchNextCubeRotation(cubeRotations, rotations, 0);
            for (var i = 0; i < rotations.Count; i++)
            {
                var faceRotation = rotations[i];
                if (faceRotation.TurnType == TurnType.Cube)
                {
                    if (i < rotations.Count - 2)
                        SearchNextCubeRotation(cubeRotations, rotations, i + 1);
                    continue;
                }

                var rotation = faceRotation.ToCubeTurnedRotation(cubeRotations);
                switch (rotation.BasicRotation)
                {
                    case BasicRotation.R:
                        switch (rotation.RotationType)
                        {
                            case Rotation.One:
                                R();
                                break;
                            case Rotation.Prime:
                                RPrime();
                                break;
                            case Rotation.Two:
                                R();
                                R();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case BasicRotation.U:
                        switch (rotation.RotationType)
                        {
                            case Rotation.One:
                                U();
                                break;
                            case Rotation.Prime:
                                UPrime();
                                break;
                            case Rotation.Two:
                                U();
                                U();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case BasicRotation.L:
                        switch (rotation.RotationType)
                        {
                            case Rotation.One:
                                L();
                                break;
                            case Rotation.Prime:
                                LPrime();
                                break;
                            case Rotation.Two:
                                L();
                                L();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case BasicRotation.F:
                        switch (rotation.RotationType)
                        {
                            case Rotation.One:
                                F();
                                break;
                            case Rotation.Prime:
                                FPrime();
                                break;
                            case Rotation.Two:
                                F();
                                F();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case BasicRotation.B:
                        switch (rotation.RotationType)
                        {
                            case Rotation.One:
                                B();
                                break;
                            case Rotation.Prime:
                                BPrime();
                                break;
                            case Rotation.Two:
                                B();
                                B();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case BasicRotation.D:
                        switch (rotation.RotationType)
                        {
                            case Rotation.One:
                                D();
                                break;
                            case Rotation.Prime:
                                DPrime();
                                break;
                            case Rotation.Two:
                                D();
                                D();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case BasicRotation.M:
                        switch (rotation.RotationType)
                        {
                            case Rotation.One:
                                R();
                                LPrime();
                                break;
                            case Rotation.Prime:
                                RPrime();
                                L();
                                break;
                            case Rotation.Two:
                                R();
                                LPrime();
                                R();
                                LPrime();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case BasicRotation.M2:
                        switch (rotation.RotationType)
                        {
                            case Rotation.One:
                                FPrime();
                                B();
                                break;
                            case Rotation.Prime:
                                F();
                                BPrime();
                                break;
                            case Rotation.Two:
                                FPrime();
                                B();
                                FPrime();
                                B();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case BasicRotation.M3:
                        switch (rotation.RotationType)
                        {
                            case Rotation.One:
                                DPrime();
                                U();
                                break;
                            case Rotation.Prime:
                                D();
                                UPrime();
                                break;
                            case Rotation.Two:
                                D();
                                UPrime();
                                D();
                                UPrime();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(rotation.BasicRotation), rotation.BasicRotation, null);
                }

                if (faceRotation.TurnType != TurnType.DoubleLayer) continue;

                CubeRotation cubeRotation;
                switch (faceRotation.DoubleLayerRotation)
                {
                    case DoubleLayerRotation.u:
                    case DoubleLayerRotation.d:
                        cubeRotation = CubeRotation.y;
                        break;
                    case DoubleLayerRotation.r:
                    case DoubleLayerRotation.l:
                        cubeRotation = CubeRotation.x;
                        break;
                    case DoubleLayerRotation.f:
                    case DoubleLayerRotation.b:
                        cubeRotation = CubeRotation.z;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var rotationType = faceRotation.RotationType switch
                {
                    Rotation.Prime => Rotation.One,
                    Rotation.One => Rotation.Prime,
                    Rotation.Two => Rotation.Two,
                    _ => throw new ArgumentOutOfRangeException()
                };

                cubeRotations.Insert(0, new FaceRotation(cubeRotation, rotationType));

            }
        }

        private void SearchNextCubeRotation(
            IList<FaceRotation> cubeRotations,
            IReadOnlyList<FaceRotation> faceRotations,
            int i)
        {
            while (faceRotations[i].TurnType != TurnType.Cube)
            {
                i++;
                if (i > faceRotations.Count - 1) return;
            }

            var faceRotation = faceRotations[i];
            
            cubeRotations.Insert(0, new FaceRotation(faceRotation.CubeRotation, faceRotation.RotationType));
        }

        public void LoadState(Face[] faces) => _faces = faces;

        public string PrintCubeState() => _faces.Aggregate("", (current, face) => current + face.PrintSide());
    }
}

using Parser;
using System;
using System.Collections.Generic;

namespace Model
{
    public class FaceRotation
    {
        public BasicRotation BasicRotation { get; set; }

        public TurnType TurnType { get; }

        public Rotation RotationType { get; set; }

        public CubeRotation CubeRotation { get; }

        public DoubleLayerRotation DoubleLayerRotation { get; set; }

        public FaceRotation(BasicRotation basicRotation, Rotation rotationType)
        {
            BasicRotation = basicRotation;
            RotationType = rotationType;
            TurnType = TurnType.Face;
        }

        public FaceRotation(CubeRotation cubeRotation, Rotation rotationType)
        {
            CubeRotation = cubeRotation;
            TurnType = TurnType.Cube;
            RotationType = rotationType;
        }

        public FaceRotation(DoubleLayerRotation doubleLayerRotation, Rotation rotationType)
        {
            DoubleLayerRotation = doubleLayerRotation;
            TurnType = TurnType.DoubleLayer;
            RotationType = rotationType;
            SetBasicRotationForDoubleLayer(doubleLayerRotation);
        }

        public FaceRotation(string notation)
        {
            if (notation[0] == 'x' || notation[0] == 'y')
            {
                Enum.TryParse(notation[0].ToString(), out CubeRotation cubeRotation);
                TurnType = TurnType.Cube;
                CubeRotation = cubeRotation;
            }
            else if (Enum.TryParse(notation[0].ToString(), out DoubleLayerRotation doubleLayerRotation))
            {
                TurnType = TurnType.DoubleLayer;
                DoubleLayerRotation = doubleLayerRotation;
                SetBasicRotationForDoubleLayer(doubleLayerRotation);

            }
            else if (notation[0] == 'M')
            {
                Enum.TryParse(notation.Substring(0, 2), out BasicRotation basicRotation);
                BasicRotation = basicRotation;
                TurnType = TurnType.Face;
            }
            else
            {
                Enum.TryParse(notation[0].ToString(), out BasicRotation basicRotation);
                BasicRotation = basicRotation;
                TurnType = TurnType.Face;
            }

            if (notation.Contains("'"))
            {
                RotationType = Rotation.Prime;
            }

            else if (notation.Contains("2") && !notation.Contains("M2"))
            {
                RotationType = Rotation.Two;
            }
            else if (notation == "M22")
            {
                RotationType = Rotation.Two;
            }
            else
            {
                RotationType = Rotation.One;
            }
        }

        private void SetBasicRotationForDoubleLayer(DoubleLayerRotation doubleLayerRotation) =>
            BasicRotation = doubleLayerRotation switch
            {
                DoubleLayerRotation.u => BasicRotation.D,
                DoubleLayerRotation.d => BasicRotation.U,
                DoubleLayerRotation.r => BasicRotation.L,
                DoubleLayerRotation.l => BasicRotation.R,
                DoubleLayerRotation.f => BasicRotation.B,
                DoubleLayerRotation.b => BasicRotation.F,
                _ => throw new ArgumentOutOfRangeException()
            };

        public FaceRotation(FaceRotation other)
        {
            BasicRotation = other.BasicRotation;
            TurnType = other.TurnType;
            CubeRotation = other.CubeRotation;
            RotationType = other.RotationType;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FaceRotation)) return false;

            var rotation = (FaceRotation)obj;

            return rotation.TurnType switch
            {
                TurnType.Face => rotation.BasicRotation == BasicRotation && rotation.RotationType == RotationType,
                TurnType.DoubleLayer => rotation.BasicRotation == BasicRotation && rotation.RotationType == RotationType,
                TurnType.Cube => rotation.CubeRotation == CubeRotation && rotation.RotationType == RotationType,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override int GetHashCode()
        {
            var hash = 12;
            hash = hash * 7 + RotationType.GetHashCode();
            hash = hash * 7 + TurnType.GetHashCode();
            return hash;
        }
    }

    public static class FaceRotationExtensions
    {
        public static string ToRubikNotation(this FaceRotation rotation)
        {
            var basicString = rotation.TurnType switch
            {
                TurnType.Face => rotation.BasicRotation.ToString(),
                TurnType.DoubleLayer => rotation.DoubleLayerRotation.ToString(),
                TurnType.Cube => rotation.CubeRotation.ToString(),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (rotation.RotationType == Rotation.One)
                return basicString;

            var extraChar = rotation.RotationType == Rotation.Prime ? "'" : "2";
            return basicString + extraChar;
        }

        public static FaceRotation ToCubeTurnedRotation(this FaceRotation rotation, List<FaceRotation> cubeRotations)
        {
            if (cubeRotations.Count == 0)
                return rotation;

            var resultRotation = new FaceRotation(rotation);
            foreach (var cubeRotation in cubeRotations)
            {
                var timesToCheck = cubeRotation.RotationType switch
                {
                    Rotation.Prime => 3,
                    Rotation.One => 1,
                    Rotation.Two => 2,
                    _ => throw new ArgumentOutOfRangeException()
                };

                for (var i = 0; i < timesToCheck; i++)
                {
                    var valueTuple = RotationEqualityTable.EqualityTable[resultRotation.BasicRotation];
                    resultRotation.BasicRotation = cubeRotation.CubeRotation == CubeRotation.x
                        ? valueTuple.xRotation
                        : valueTuple.yRotation;
                }
            }

            if (rotation.TurnType == TurnType.DoubleLayer)
            {
                resultRotation.DoubleLayerRotation = rotation.DoubleLayerRotation;
            }

            return resultRotation;
        }

        public static List<FaceRotation> ReverseAlg(this List<FaceRotation> rotations)
        {
            for (var i = 0; i < rotations.Count; i++)
            {
                var rotation = rotations[i];
                if (rotation.RotationType == Rotation.Two) continue;

                rotation.RotationType = rotation.RotationType == Rotation.One ? Rotation.Prime : Rotation.One;

                rotations[i] = rotation;
            }

            rotations.Reverse();
            return rotations;
        }

        public static List<FaceRotation> ReverseCubeRotations(this List<FaceRotation> rotations)
        {
            foreach (var rotation in rotations)
            {
                if (rotation.RotationType == Rotation.Two || rotation.TurnType != TurnType.Cube) continue;

                rotation.RotationType = rotation.RotationType == Rotation.One ? Rotation.Prime : Rotation.One;
            }

            rotations.Reverse();
            return rotations;
        }
    }
}

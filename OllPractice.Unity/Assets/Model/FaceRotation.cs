using Parser;
using System;
using System.Collections.Generic;

namespace Model
{
    public class FaceRotation
    {
        public BasicRotation BasicRotation { get; set; }

        public CubeRotation CubeRotation { get; }

        public Rotation RotationType { get; }

        public bool IsCubeRotation { get; }

        public FaceRotation(BasicRotation basicRotation, Rotation rotationType)
        {
            BasicRotation = basicRotation;
            RotationType = rotationType;
        }

        public FaceRotation(CubeRotation cubeRotation, Rotation rotationType)
        {
            CubeRotation = cubeRotation;
            RotationType = rotationType;
            IsCubeRotation = true;
        }

        public FaceRotation(string notation)
        {
            if (notation[0] == 'x' || notation[0] == 'y')
            {
                Enum.TryParse(notation[0].ToString(), out CubeRotation cubeRotation);
                IsCubeRotation = true;
                CubeRotation = cubeRotation;
            }
            else if (notation[0] != 'M')
            {
                Enum.TryParse(notation[0].ToString(), out BasicRotation basicRotation);
                BasicRotation = basicRotation;
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

        public FaceRotation(FaceRotation other)
        {
            BasicRotation = other.BasicRotation;
            CubeRotation = other.CubeRotation;
            IsCubeRotation = other.IsCubeRotation;
            RotationType = other.RotationType;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FaceRotation)) return false;

            var rotation = (FaceRotation)obj;
            if (rotation.IsCubeRotation)
                return IsCubeRotation &&
                       rotation.CubeRotation == CubeRotation &&
                       rotation.RotationType == RotationType;

            return !IsCubeRotation &&
                   rotation.BasicRotation == BasicRotation &&
                   rotation.RotationType == RotationType;

        }

        public override int GetHashCode()
        {
            var hash = 12;
            hash = hash * 7 + RotationType.GetHashCode();
            hash = hash * 7 + CubeRotation.GetHashCode();
            return hash;
        }
    }

    public static class FaceRotationExtensions
    {
        public static string ToRubikNotation(this FaceRotation rotation)
        {
            var basicString = rotation.IsCubeRotation
                ? rotation.CubeRotation.ToString()
                : rotation.BasicRotation.ToString();

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

            return resultRotation;
        }
    }
}

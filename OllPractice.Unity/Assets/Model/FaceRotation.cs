using System;
using System.Collections.Generic;

namespace Model
{
    public class FaceRotation
    {
        public BasicRotation BasicRotation { get; }

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
            Enum.TryParse(notation[0].ToString(), out BasicRotation basicRotation);
            BasicRotation = basicRotation;
            if (notation.Length == 1)
            {
                RotationType = Rotation.One;
                return;
            }

            var rotationType = notation[1] == '2' ? Rotation.Two : Rotation.Prime;
            RotationType = rotationType;
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
            hash = hash * 7 + BasicRotation.GetHashCode();
            hash = hash * 7 + RotationType.GetHashCode();
            hash = hash * 7 + CubeRotation.GetHashCode();
            return hash;
        }
    }

    public static class FaceRotationExtensions
    {
        public static string ToRubikNotation(this FaceRotation rotation)
        {
            if (rotation.RotationType == Rotation.One)
                return rotation.BasicRotation.ToString();

            var extraChar = rotation.RotationType == Rotation.Prime ? "'" : "2";
            return rotation.BasicRotation + extraChar;
        }

        public static FaceRotation ToCubeTurnedRotation(this FaceRotation rotation, List<FaceRotation> rotations)
        {
            if (rotations.Count == 0)
                return rotation;
            else
            {
                return rotation;
            }
        }
    }
}

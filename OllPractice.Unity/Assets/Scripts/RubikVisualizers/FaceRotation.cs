using System;

namespace RubikVisualizers
{
    public enum FaceRotation
    {
        R,
        RPrime,
        U,
        UPrime,
        L,
        LPrime,
        F,
        FPrime,
        B,
        BPrime,
        D,
        DPrime,
        M,
        MPrime,
        M2,
        M2Prime,
        M3,
        M3Prime,
    }

    public static class FaceRotationExtensions
    {
        public static string ToRubikNotation(this FaceRotation rotation)
        {
            switch (rotation)
            {
                case FaceRotation.R:
                    return "R";
                case FaceRotation.RPrime:
                    return "R'";
                case FaceRotation.U:
                    return "U";
                case FaceRotation.UPrime:
                    return "U'";
                case FaceRotation.L:
                    return "L";
                case FaceRotation.LPrime:
                    return "L'";
                case FaceRotation.F:
                    return "F";
                case FaceRotation.FPrime:
                    return "F'";
                case FaceRotation.B:
                    return "B";
                case FaceRotation.BPrime:
                    return "B'";
                case FaceRotation.D:
                    return "D";
                case FaceRotation.DPrime:
                    return "D'";
                case FaceRotation.M:
                    return "M";
                case FaceRotation.MPrime:
                    return "M'";
                case FaceRotation.M2:
                    return "M2";
                case FaceRotation.M2Prime:
                    return "M2'";
                case FaceRotation.M3:
                    return "M3";
                case FaceRotation.M3Prime:
                    return "M3'";
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
            }
        }
    }
}

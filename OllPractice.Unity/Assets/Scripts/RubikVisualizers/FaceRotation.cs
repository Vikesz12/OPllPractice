using System;
using Model;

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

        public static FaceRotation ToF2LRotation(this FaceRotation rotation)
        {
            switch (rotation)
            {
                case FaceRotation.R:
                    return FaceRotation.F;
                case FaceRotation.RPrime:
                    return FaceRotation.FPrime;
                case FaceRotation.U:
                    return FaceRotation.D;
                case FaceRotation.UPrime:
                    return FaceRotation.DPrime;
                case FaceRotation.L:
                    return FaceRotation.B;
                case FaceRotation.LPrime:
                    return FaceRotation.BPrime;
                case FaceRotation.F:
                    return FaceRotation.R;
                case FaceRotation.FPrime:
                    return FaceRotation.RPrime;
                case FaceRotation.B:
                    return FaceRotation.L;
                case FaceRotation.BPrime:
                    return FaceRotation.LPrime;
                case FaceRotation.D:
                    return FaceRotation.U;
                case FaceRotation.DPrime:
                    return FaceRotation.UPrime;
                case FaceRotation.M:
                    return FaceRotation.M2;
                case FaceRotation.MPrime:
                    return FaceRotation.M2Prime;
                case FaceRotation.M2:
                    return FaceRotation.M;
                case FaceRotation.M2Prime:
                    return FaceRotation.MPrime;
                case FaceRotation.M3:
                    return FaceRotation.M3;
                case FaceRotation.M3Prime:
                    return FaceRotation.M3Prime;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
            }
        }
    }
}

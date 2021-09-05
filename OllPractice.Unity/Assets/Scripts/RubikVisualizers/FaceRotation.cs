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
        Y,
        YPrime
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
                case FaceRotation.Y:
                    return "y";
                case FaceRotation.YPrime:
                    return "y'";
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
            }
        }

        public static FaceRotation ToF2LRotation(this FaceRotation rotation, int yTurns)
        {
            var sideRotationArray = new[] { FaceRotation.F, FaceRotation.L, FaceRotation.B, FaceRotation.R };
            var primeSideRotationArray = new[] { FaceRotation.FPrime, FaceRotation.LPrime, FaceRotation.BPrime, FaceRotation.RPrime };
            var mRotationArray = new[] { FaceRotation.M2, FaceRotation.MPrime, FaceRotation.M2Prime, FaceRotation.M };
            var yValue = yTurns < 0 ? 4 - Math.Abs(yTurns % 4) : yTurns % 4;
            switch (rotation)
            {
                case FaceRotation.R:
                    return sideRotationArray[yValue];
                case FaceRotation.B:
                    return sideRotationArray[(1 + yValue) % 4];
                case FaceRotation.L:
                    return sideRotationArray[(2 + yValue) % 4];
                case FaceRotation.F:
                    return sideRotationArray[(3 + yValue) % 4];
                case FaceRotation.RPrime:
                    return primeSideRotationArray[yValue];
                case FaceRotation.BPrime:
                    return primeSideRotationArray[(1 + yValue) % 4];
                case FaceRotation.LPrime:
                    return primeSideRotationArray[(2 + yValue) % 4];
                case FaceRotation.FPrime:
                    return primeSideRotationArray[(3 + yValue) % 4];
                case FaceRotation.U:
                    return FaceRotation.D;
                case FaceRotation.UPrime:
                    return FaceRotation.DPrime;
                case FaceRotation.D:
                    return FaceRotation.U;
                case FaceRotation.DPrime:
                    return FaceRotation.UPrime;
                case FaceRotation.M:
                    return mRotationArray[yValue];
                case FaceRotation.MPrime:
                    return mRotationArray[(2 + yValue) % 4];
                case FaceRotation.M2:
                    return mRotationArray[(3 + yValue) % 4];
                case FaceRotation.M2Prime:
                    return mRotationArray[(1 + yValue) % 4];
                case FaceRotation.M3:
                    return FaceRotation.M3;
                case FaceRotation.M3Prime:
                    return FaceRotation.M3Prime;
                case FaceRotation.Y:
                    return FaceRotation.YPrime;
                case FaceRotation.YPrime:
                    return FaceRotation.Y;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
            }
        }
    }
}

using Model;
using RubikVisualizers;
using System;
using System.Collections.Generic;
using TwoPhaseSolver;

namespace Solver
{
    public static class KociembaSolver
    {
        public static FaceRotation[] SolveCube(Face[] currentFaces)
        {
            var faceletBytes = new List<byte>();
            foreach (var face in currentFaces)
            {
                faceletBytes.AddRange(face.GetAllColors());
            }

            var cube = new TwoPhaseSolver.Cube(faceletBytes.ToArray());
            var move = Search.fullSolve(cube, 22);

            return MoveListToFaceRotation(move);
        }

        private static FaceRotation[] MoveListToFaceRotation(Move move)
        {
            var result = new List<FaceRotation>();
            foreach (var rotationString in move.ToString().Split(' '))
            {
                switch (rotationString)
                {
                    case "U":
                        result.Add(FaceRotation.U);
                        break;
                    case "U2":
                        result.Add(FaceRotation.U);
                        result.Add(FaceRotation.U);
                        break;
                    case "U'":
                        result.Add(FaceRotation.UPrime);
                        break;
                    case "R":
                        result.Add(FaceRotation.R);
                        break;
                    case "R2":
                        result.Add(FaceRotation.R);
                        result.Add(FaceRotation.R);
                        break;
                    case "R'":
                        result.Add(FaceRotation.RPrime);
                        break;
                    case "F":
                        result.Add(FaceRotation.F);
                        break;
                    case "F2":
                        result.Add(FaceRotation.F);
                        result.Add(FaceRotation.F);
                        break;
                    case "F'":
                        result.Add(FaceRotation.FPrime);
                        break;
                    case "D":
                        result.Add(FaceRotation.D);
                        break;
                    case "D2":
                        result.Add(FaceRotation.D);
                        result.Add(FaceRotation.D);
                        break;
                    case "D'":
                        result.Add(FaceRotation.DPrime);
                        break;
                    case "L":
                        result.Add(FaceRotation.L);
                        break;
                    case "L2":
                        result.Add(FaceRotation.L);
                        result.Add(FaceRotation.L);
                        break;
                    case "L'":
                        result.Add(FaceRotation.LPrime);
                        break;
                    case "B":
                        result.Add(FaceRotation.B);
                        break;
                    case "B2":
                        result.Add(FaceRotation.B);
                        result.Add(FaceRotation.B);
                        break;
                    case "B'":
                        result.Add(FaceRotation.BPrime);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(rotationString), "rotation string unknown");
                }
            }

            return result.ToArray();
        }
    }
}

using System;
using System.Collections.Generic;
using Model;
using RubikVisualizers;
using TwoPhaseSolver;
using Cube = Model.Cube;

namespace Solver
{
    public static class KociembaSolver
    {
        public static FaceRotation[] SolveCube(Face[] currentFaces)
        {
            var faceletBytes = new List<byte>();
            faceletBytes.AddRange(ToSolverColor(currentFaces[0].GetAllColors()));
            faceletBytes.AddRange(ToSolverColor(currentFaces[3].GetAllColors()));
            faceletBytes.AddRange(ToSolverColor(currentFaces[1].GetAllColors()));
            faceletBytes.AddRange(ToSolverColor(currentFaces[2].GetAllColors()));
            faceletBytes.AddRange(ToSolverColor(currentFaces[4].GetAllColors()));
            faceletBytes.AddRange(ToSolverColor(currentFaces[5].GetAllColors()));

            var cube = new TwoPhaseSolver.Cube(faceletBytes.ToArray());
            var move = Search.fullSolve(cube, 22);

            return MoveListToFaceRotation(move);
        }

        private static IEnumerable<byte> ToSolverColor(IEnumerable<byte> colors)
        {
            var result = new List<byte>();
            foreach (var color in colors)
            {
                switch (color)
                {
                    case 1:
                        result.Add(2);
                        break;
                    case 2:
                        result.Add(3);
                        break;
                    case 3:
                        result.Add(1);
                        break;
                    default:
                        result.Add(color);
                        break;
                }
            }

            return result.ToArray();
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

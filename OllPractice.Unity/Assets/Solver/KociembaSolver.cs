using Model;
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
                if(rotationString == "None") continue;
                result.Add(new FaceRotation(rotationString));
            }

            return result.ToArray();
        }
    }
}

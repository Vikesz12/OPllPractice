using Model;
using UnityEngine;

namespace RubikVisualizers
{
    public interface ISetFaceColor
    {
        void SetFaceColorForFacing(Vector3 facing, Material materialToSet);
        RubikColor GetFaceColorForFacing(Vector3 facing);
    }
}
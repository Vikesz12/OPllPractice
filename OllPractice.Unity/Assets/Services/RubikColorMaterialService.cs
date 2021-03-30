using System;
using Model;
using UnityEngine;

namespace Services
{
    public static class RubikColorMaterialService
    {
        public static Material GetRubikColorMaterial(RubikColor color)
        {
            var materialName = "Materials/";
            switch (color)
            {
                case RubikColor.W:
                    materialName += "White";
                    break;
                case RubikColor.G:
                    materialName += "Green";
                    break;
                case RubikColor.R:
                    materialName += "Red";
                    break;
                case RubikColor.B:
                    materialName += "Blue";
                    break;
                case RubikColor.O:
                    materialName += "Orange";
                    break;
                case RubikColor.Y:
                    materialName += "Yellow";
                    break;
                case RubikColor.L:
                    materialName += "Black";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }

            return Resources.Load<Material>(materialName);
        }

    }
}

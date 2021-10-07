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
            materialName += color switch
            {
                RubikColor.W => "White",
                RubikColor.G => "Green",
                RubikColor.R => "Red",
                RubikColor.B => "Blue",
                RubikColor.O => "Orange",
                RubikColor.Y => "Yellow",
                RubikColor.L => "Black",
                _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
            };

            return Resources.Load<Material>(materialName);
        }

        public static RubikColor GetMaterialColor(Material material) =>
            material.name.Split(' ')[0] switch
            {
                "White" => RubikColor.W,
                "Green" => RubikColor.G,
                "Red" => RubikColor.R,
                "Orange" => RubikColor.O,
                "Blue" => RubikColor.B,
                "Yellow" => RubikColor.Y,
                "Black" => RubikColor.L,
                _ => throw new ArgumentOutOfRangeException(nameof(material), material.name, null)
            };
    }
}

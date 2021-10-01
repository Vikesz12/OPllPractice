using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.Common.Merge;
using UnityEngine;

namespace Parser
{
    public static class RotationEqualityTable
    {
        public static Dictionary<BasicRotation, (BasicRotation xRotation, BasicRotation yRotation)> EqualityTable
            = new Dictionary<BasicRotation, (BasicRotation xRotation, BasicRotation yRotation)>();

        private static bool Loaded;

        public static void LoadData()
        {
            if(Loaded) return;
            Loaded = true;

            var textAsset = Resources.Load<TextAsset>("Equality/RotationTable");
            var lines = textAsset.text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                var rotations = line.Split(',').Select(r => new FaceRotation(r)).ToArray();
                EqualityTable.Add(rotations[0].BasicRotation,(rotations[1].BasicRotation,rotations[2].BasicRotation));
            }
        }
    }
}

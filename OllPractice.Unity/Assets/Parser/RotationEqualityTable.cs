using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Parser
{
    public static class RotationEqualityTable
    {
        public static Dictionary<BasicRotation, (BasicRotation xRotation, BasicRotation yRotation, BasicRotation zRotation)> EqualityTable
            = new Dictionary<BasicRotation, (BasicRotation xRotation, BasicRotation yRotation, BasicRotation zRotation)>();

        private static bool _loaded;

        public static void LoadData()
        {
            if(_loaded) return;
            _loaded = true;

            var textAsset = Resources.Load<TextAsset>("Equality/RotationTable");
            var lines = textAsset.text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                var rotations = line.Split(',').Select(r => new FaceRotation(r)).ToArray();
                EqualityTable.Add(rotations[0].BasicRotation,(rotations[1].BasicRotation,rotations[2].BasicRotation, rotations[3].BasicRotation));
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Model;
using TMPro;
using UnityEngine;

namespace View
{
    public class FaceView
    {
        private readonly GameObject _center;
        private readonly List<GameObject> _cubes;

        public FaceView(GameObject center)
        {
            _center = center;
            _cubes = new List<GameObject>(8);
        }

        public void AddCube(GameObject cube)
        {
            _cubes.Add(cube);
        }

        public void RotateFace(Rotation rot)
        {
            foreach (var cube in _cubes)
            {
                cube.transform.parent = _center.transform;
            }
            _center.transform.RotateAround(_center.transform.position, _center.transform.up, rot == Rotation.ONE ? 90 : -90);
        }

        public void ResetFaceParents(RubikVisualizer rubikVisualizer)
        {
            foreach (var cube in _cubes)
            {
                cube.transform.parent = rubikVisualizer.transform;
            }
        }
    }
}

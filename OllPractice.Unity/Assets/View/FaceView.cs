using System;
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
        public List<GameObject> Cubes { get; }

        public FaceView(GameObject center)
        {
            _center = center;
            Cubes = new List<GameObject>(8);
        }

        public void AddCube(GameObject cube)
        {
            Cubes.Add(cube);
        }

        public void RotateFace(Rotation rot)
        {
            foreach (var cube in Cubes)
            {
                cube.transform.parent = _center.transform;
            }
            _center.transform.RotateAround(_center.transform.position, _center.transform.up, rot == Rotation.ONE ? 90 : -90);
        }

        public void ResetFaceParents(RubikVisualizer rubikVisualizer)
        {
            foreach (var cube in Cubes)
            {
                cube.transform.parent = rubikVisualizer.transform;
            }
        }

        public IEnumerable<GameObject> RemoveCubes(IEnumerable<GameObject> cubes)
        {
            var cubesToRemove = cubes.Where(c => Cubes.Contains(c)).ToList();
            Cubes.RemoveAll(cubesToRemove.Contains);
            return cubesToRemove;
        }

        public void AddCubes(IEnumerable<GameObject> cubes)
        {
            foreach (var cube in cubes)
            {
                Cubes.Add(cube);
            }
        }
    }
}

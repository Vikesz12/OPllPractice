using System;
using Model;
using RubikVisualizers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace View
{
    public class FaceView : MonoBehaviour
    {
        private GameObject _center;
        private float _timeToRotate;
        private float _angleToRotate;
        private float _currentTime;
        public List<GameObject> Cubes { get; private set; }

        private void Start()
        {
            _center = gameObject;
            Cubes = new List<GameObject>(8);
        }

        public void AddCube(GameObject cube)
        {
            Cubes.Add(cube);
        }

        public IEnumerator RotateFace(Rotation rot, RubikVisualizer visualizer, Action coroutineCallback)
        {
            return RotateCoroutine(rot, visualizer, coroutineCallback);
        }

        IEnumerator RotateCoroutine(Rotation rot, RubikVisualizer rubikVisualizer, Action callback)
        {
            foreach (var cube in Cubes)
            {
                cube.transform.parent = _center.transform;
            }

            _currentTime = 0.0f;
             _angleToRotate = rot == Rotation.ONE ? 90f : -90f;
            _timeToRotate = 0.25f;
            while (_currentTime < _timeToRotate)
            {
                _currentTime += Time.deltaTime;
                _center.transform.RotateAround(_center.transform.position, _center.transform.up, _angleToRotate * Time.deltaTime/_timeToRotate);
                yield return null;
            }
            callback.Invoke();
        }

        
        public void FinishCoroutine(RubikVisualizer visualizer)
        {
            _center.transform.RotateAround(_center.transform.position, _center.transform.up, _angleToRotate * (_timeToRotate-_currentTime) / _timeToRotate);
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

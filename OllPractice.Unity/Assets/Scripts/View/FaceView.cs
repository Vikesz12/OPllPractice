using Model;
using System;
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

        private void Awake()
        {
            _center = gameObject;
            Cubes = new List<GameObject>(8);
        }

        public Vector3 Facing => _center.transform.up;

        public void AddCube(GameObject cube)
        {
            Cubes.Add(cube);
        }

        public IEnumerator RotateCoroutine(Rotation rot, Action callback)
        {
            SetCubeParents();

            _currentTime = 0.0f;
            _angleToRotate = rot == Rotation.ONE ? 90f : -90f;
            _timeToRotate = 0.25f;
            while (_currentTime <= _timeToRotate)
            {
                _currentTime += Time.deltaTime;
                _center.transform.RotateAround(_center.transform.position, _center.transform.up, _angleToRotate * (Time.deltaTime / _timeToRotate));
                yield return null;
            }
            Debug.Log($"{_center.transform.eulerAngles} angles after rotation");
            callback.Invoke();
        }

        private void SetCubeParents()
        {
            foreach (var cube in Cubes)
            {
                cube.transform.parent = _center.transform;
            }
        }

        public void RotateWithoutAnimation(Rotation rot)
        {
            SetCubeParents();
            _center.transform.RotateAround(_center.transform.position, _center.transform.up, rot == Rotation.ONE ? 90 : -90);
        }

        public void SkipCoroutine()
        {
            _center.transform.RotateAround(_center.transform.position, _center.transform.up, _angleToRotate * (_timeToRotate - _currentTime) / _timeToRotate);
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

        public int GetCubeIndex(Vector3 cubePosition)
        {
            var relativePos = _center.transform.InverseTransformPoint(cubePosition);
            const float epsilon = 0.005f;
            if (relativePos.x < -epsilon)
            {
                if (Math.Abs(relativePos.z) < epsilon)
                    return 7;
                return relativePos.z > 0 ? 0 : 6;
            }

            if (relativePos.x > epsilon)
            {
                if (Math.Abs(relativePos.z) < epsilon)
                    return 3;
                return relativePos.z > 0 ? 2 : 4;
            }

            return relativePos.z > 0 ? 1 : 5;
        }

        private bool PositionEquals(float a, float b)
        {
            const float epsilon = 0.005f;
            return Math.Abs(a - b) < epsilon;
        }
    }
}

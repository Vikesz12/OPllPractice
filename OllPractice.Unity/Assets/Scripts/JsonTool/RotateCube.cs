using System;
using UnityEngine;
using UnityEngine.UI;

namespace JsonTool
{
    [RequireComponent(typeof(Button))]
    public class RotateCube : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotation;
        [SerializeField] private GameObject _cubeToRotate;
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnRotateClicked);
        }

        private void OnRotateClicked() => _cubeToRotate.transform.eulerAngles += _rotation;
    }
}

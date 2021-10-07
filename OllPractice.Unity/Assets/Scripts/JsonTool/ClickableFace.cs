using RubikVisualizers;
using UnityEngine;
using View;

namespace JsonTool
{
    [RequireComponent(typeof(BoxCollider), typeof(MeshRenderer))]
    public class ClickableFace : MonoBehaviour
    {
        private MeshRenderer _renderer;
        private FaceView _faceView;
        private RubikVisualizer _visualizer;
        private GameObject _faceParent;
        private GameObject _rubikObject;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _visualizer = GetComponentInParent<RubikVisualizer>();
            var rubikObject = transform.parent;
            while (rubikObject.parent != rubikObject.root)
            {
                rubikObject = rubikObject.parent;
            }

            _rubikObject = rubikObject.gameObject;
        }

        private void OnMouseDown()
        {
            var faceView = _visualizer.GetFaceViewForCube(_rubikObject);
            var faceParent = faceView.gameObject;
            var cubeIndex = faceView.GetCubeIndex(transform.parent.parent.position);
            _visualizer.GetCurrentCube.GetFaces[faceParent.transform.GetSiblingIndex()].SetColorAt(cubeIndex, JsonToolSettings.CurrentColor);
            _renderer.material = JsonToolSettings.CurrentMaterial;
        }
    }
}

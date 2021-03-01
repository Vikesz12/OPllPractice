using System.Collections;
using System.Collections.Generic;
using Model;
using Parser;
using UnityEngine;
using View;

public class RubikVisualizer : MonoBehaviour
{
    private NotificationParser _notificationParse;
    private Cube _cube;
    private List<FaceView> _faces;

    public void Start()
    {
        _notificationParse = new NotificationParser();
        _cube = new Cube();
        _faces = new List<FaceView>();
        for (var i = 0; i < 6; i++)
        {
            var newFace = new FaceView(transform.GetChild(i).gameObject);
            _faces.Add(newFace);
        }

        SetFaceReferences();
    }

    private void SetFaceReferences()
    {
        for (var i = 0; i < 4; i++)
        {
            const int startIndex = 6;
            var whiteEdge = transform.GetChild(startIndex + i).gameObject;
            _faces[0].AddCube(whiteEdge);
            _faces[i + 1].AddCube(whiteEdge);
        }

        for (var i = 0; i < 4; i++)
        {
            const int startIndex = 10;
            var middleEdges = transform.GetChild(startIndex + i).gameObject;
            _faces[i + 1].AddCube(middleEdges);
            switch (i)
            {
                case 0:
                    _faces[3].AddCube(middleEdges);
                    break;

                case 1:
                    _faces[1].AddCube(middleEdges);
                    break;

                case 2:
                    _faces[4].AddCube(middleEdges);
                    break;

                case 3:
                    _faces[2].AddCube(middleEdges);
                    break;
            }
        }

        for (var i = 0; i < 4; i++)
        {
            const int startIndex = 14;
            var yellowEdge = transform.GetChild(startIndex + i).gameObject;
            _faces[5].AddCube(yellowEdge);
            _faces[i + 1].AddCube(yellowEdge);

        }

        for (var i = 0; i < 4; i++)
        {
            const int startIndex = 18;
            var topCorners = transform.GetChild(startIndex + i).gameObject;
            _faces[0].AddCube(topCorners);
            _faces[i > 1 ? 4 : 1].AddCube(topCorners);
            _faces[i % 2 == 0 ? 2 : 3].AddCube(topCorners);
        }

        for (var i = 0; i < 4; i++)
        {
            const int startIndex = 22;
            var bottomCorners = transform.GetChild(startIndex + i).gameObject;
            _faces[5].AddCube(bottomCorners);
            _faces[i > 1 ? 4 : 1].AddCube(bottomCorners);
            _faces[i % 2 == 0 ? 2 : 3].AddCube(bottomCorners);
        }
    }


    public void ProcessMessage(byte[] notification) => _notificationParse.ParseNotification(notification, _cube, this);

    public void U() => RotateSide(0, 1, 2, 4, 3, Rotation.ONE);

    public void UPrime() => RotateSide(0, 1, 3, 4, 2, Rotation.PRIME);

    public void R() => RotateSide(3, 0, 4, 5, 1, Rotation.ONE);

    public void RPrime() => RotateSide(3, 0, 1, 5, 4, Rotation.PRIME);


    public void L() => RotateSide(2, 0, 1, 5, 4, Rotation.ONE);

    public void LPrime() => RotateSide(2, 0, 4, 5, 1, Rotation.PRIME);

    public void F() => RotateSide(1, 0, 3, 5, 2, Rotation.ONE);

    public void FPrime() => RotateSide(1, 0, 2, 5, 3, Rotation.PRIME);

    public void B() => RotateSide(4, 0, 2, 5, 3, Rotation.ONE);

    public void BPrime() => RotateSide(4,0,3,5,2, Rotation.PRIME);

    public void D() => RotateSide(5,1,3,4,2, Rotation.ONE);

    public void DPrime() => RotateSide(5,1,2,4,3, Rotation.PRIME);

    private void RotateSide(int sideToRotate, int side1, int side2, int side3, int side4, Rotation rotation)
    {
        _faces[sideToRotate].RotateFace(rotation);
        var firstSideCubes = _faces[side1].RemoveCubes(_faces[sideToRotate].Cubes);
        var secondSideCubes = _faces[side2].RemoveCubes(_faces[sideToRotate].Cubes);
        var thirdSideCubes = _faces[side3].RemoveCubes(_faces[sideToRotate].Cubes);
        var redSideCubes = _faces[side4].RemoveCubes(_faces[sideToRotate].Cubes);
        _faces[side2].AddCubes(firstSideCubes);
        _faces[side3].AddCubes(secondSideCubes);
        _faces[side4].AddCubes(thirdSideCubes);
        _faces[side1].AddCubes(redSideCubes);
        _faces[sideToRotate].ResetFaceParents(this);
    }
}

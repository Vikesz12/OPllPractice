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


    public void ProcessMessage(byte[] notification)
    {
        _notificationParse.ParseNotification(notification, _cube, this);
    }

    public void U()
    {
        _faces[0].RotateFace(Rotation.ONE);
        _faces[0].ResetFaceParents(this);
    }
}

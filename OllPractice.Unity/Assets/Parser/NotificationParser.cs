using System;
using System.Collections.Generic;
using Model;
using OllPractice.Model.Model;
using UnityEngine;

namespace Parser
{
    public class NotificationParser
    {
        private readonly Cube _cube;

        public NotificationParser()
        {
            _cube = new Cube();
        }
        public string ParseNotification(byte[] notification)
        {
            var messageType = notification[2];

            switch (messageType)
            {
                case 1:
                    return ParseFaceRotation(notification);
                case 2:
                    return ParseState(notification);
                default:
                    throw new ArgumentOutOfRangeException(nameof(notification), "notification type unknown");
            }
        }

        private string ParseFaceRotation(byte[] notification)
        {
            switch (notification[3])
            {
                case 0:
                    _cube.B();
                    break;
                case 1:
                    _cube.BPrime();
                    break;
                case 2:
                    _cube.F();
                    break;
                case 3:
                    _cube.FPrime();
                    break;
                case 4:
                    _cube.U();
                    break;
                case 5:
                    _cube.UPrime();
                    break;
                case 6:
                    _cube.D();
                    break;
                case 7:
                    _cube.DPrime();
                    break;
                case 8:
                    _cube.R();
                    break;
                case 9:
                    _cube.RPrime();
                    break;
                case 10:
                    _cube.L();
                    break;
                case 11:
                    _cube.LPrime();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(notification), "rotation unknown");
            }

            return _cube.PrintCubeState();
        }
        private static RubikColor ParseNotificationColor(byte col)
        {
            switch (col)
            {
                case 0:
                    return RubikColor.B;
                case 1:
                    return RubikColor.G;
                case 2:
                    return RubikColor.W;
                case 3:
                    return RubikColor.Y;
                case 4:
                    return RubikColor.R;
                case 5:
                    return RubikColor.O;
                default:
                    throw new ArgumentOutOfRangeException(nameof(col), "color unknown");
            }
        }

        private string ParseState(byte[] notification)
        {
            var startIndex = 3;
            var faces = new Face[6];
            for (var i = 0; i < 6; i++)
            {
                var faceColor = notification[startIndex + i * 9];
                var faceModelColor = ParseNotificationColor(faceColor);
                var faceColors = new RubikColor[8];
                for (var j = 1; j < 9; j++)
                {
                    var rubikColor = ParseNotificationColor(notification[startIndex + i * 9 + j]);
                    if (j < 5)
                    {
                        faceColors[4 - j] = rubikColor;
                    }
                    else
                    {
                        faceColors[12 - j] = rubikColor;
                    }
                }

                var enumInt = (int)faceModelColor;
                faces[enumInt] = new Face(faceColors);
            }

            faces[0].Rotate(Rotation.ONE);
            faces[1].Rotate(Rotation.TWO);
            faces[2].Rotate(Rotation.TWO);
            faces[3].Rotate(Rotation.TWO);
            faces[4].Rotate(Rotation.TWO);
            faces[5].Rotate(Rotation.ONE);
            _cube.LoadState(faces);
            return _cube.PrintCubeState();
        }


    }
}

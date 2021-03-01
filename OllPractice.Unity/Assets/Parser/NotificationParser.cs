using System;
using System.Collections.Generic;
using Model;
using UnityEngine;

namespace Parser
{
    public class NotificationParser
    {
        public void ParseNotification(byte[] notification, Cube cube, RubikVisualizer rubikVisualizer)
        {
            var messageType = notification[2];

            switch (messageType)
            {
                case 1:
                    ParseFaceRotation(notification, cube, rubikVisualizer);
                    break;
                case 2:
                    ParseState(notification, cube);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(notification), "notification type unknown");
            }
        }

        private void ParseFaceRotation(byte[] notification, Cube cube, RubikVisualizer rubikVisualizer)
        {
            switch (notification[3])
            {
                case 0:
                    cube.B();
                    rubikVisualizer.B();
                    break;
                case 1:
                    cube.BPrime();
                    rubikVisualizer.BPrime();
                    break;
                case 2:
                    cube.F();
                    rubikVisualizer.F();
                    break;
                case 3:
                    cube.FPrime();
                    rubikVisualizer.FPrime();
                    break;
                case 4:
                    cube.U();
                    rubikVisualizer.U();
                    break;
                case 5:
                    cube.UPrime();
                    rubikVisualizer.UPrime();
                    break;
                case 6:
                    cube.D();
                    rubikVisualizer.D();
                    break;
                case 7:
                    cube.DPrime();
                    rubikVisualizer.DPrime();
                    break;
                case 8:
                    cube.R();
                    rubikVisualizer.R();
                    break;
                case 9:
                    cube.RPrime();
                    rubikVisualizer.RPrime();
                    break;
                case 10:
                    cube.L();
                    rubikVisualizer.L();
                    break;
                case 11:
                    cube.LPrime();
                    rubikVisualizer.LPrime();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(notification), "rotation unknown");
            }
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

        private void ParseState(byte[] notification, Cube cube)
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

            cube.LoadState(faces);
        }


    }
}

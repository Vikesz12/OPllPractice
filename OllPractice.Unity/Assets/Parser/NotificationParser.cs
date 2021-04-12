using Model;
using RubikVisualizers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MainThreadDispatcher;
using MainThreadDispatcher.Unity;
using UnityEngine;

namespace Parser
{
    public class NotificationParser
    {
        public void ParseNotification(byte[] notification, short dataSize)
        {
            var messageType = notification[2];

            switch (messageType)
            {
                case 1:
                    ParseFaceRotation(notification);
                    break;
                case 2:
                    ParseState(notification, dataSize);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(notification), "notification type unknown");
            }
        }

        public event Action<FaceRotation> FaceRotated;
        private void ParseFaceRotation(byte[] notification)
        {
            if (notification[1] == 8)
            {
                switch (notification[3])
                {
                    case 0:
                        FaceRotated?.Invoke(FaceRotation.M2);
                        break;
                    case 1:
                        FaceRotated?.Invoke(FaceRotation.M2Prime);
                        break;
                    case 4:
                        FaceRotated?.Invoke(FaceRotation.M3);
                        break;
                    case 5:
                        FaceRotated?.Invoke(FaceRotation.M3Prime);
                        break;
                    case 8:
                        FaceRotated?.Invoke(FaceRotation.M);
                        break;
                    case 9:
                        FaceRotated?.Invoke(FaceRotation.MPrime);
                        break;
                }
            }
            else
            {
                switch (notification[3])
                {
                    case 0:
                        FaceRotated?.Invoke(FaceRotation.B);
                        break;
                    case 1:
                        FaceRotated?.Invoke(FaceRotation.BPrime);
                        break;
                    case 2:
                        FaceRotated?.Invoke(FaceRotation.F);
                        break;
                    case 3:
                        FaceRotated?.Invoke(FaceRotation.FPrime);
                        break;
                    case 4:
                        FaceRotated?.Invoke(FaceRotation.U);
                        break;
                    case 5:
                        FaceRotated?.Invoke(FaceRotation.UPrime);
                        break;
                    case 6:
                        FaceRotated?.Invoke(FaceRotation.D);
                        break;
                    case 7:
                        FaceRotated?.Invoke(FaceRotation.DPrime);
                        break;
                    case 8:
                        FaceRotated?.Invoke(FaceRotation.R);
                        break;
                    case 9:
                        FaceRotated?.Invoke(FaceRotation.RPrime);
                        break;
                    case 10:
                        FaceRotated?.Invoke(FaceRotation.L);
                        break;
                    case 11:
                        FaceRotated?.Invoke(FaceRotation.LPrime);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(notification), "rotation unknown");
                }
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

        public event Action<Face[]> StateParsed;

        private void ParseState(byte[] notification, short dataSize)
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

            StateParsed?.Invoke(faces);
            Debug.Log("State parsed");
        }

        public async Task AnimateRotations(IEnumerable<FaceRotation> rotations)
        {
            IMainThreadDispatcher dispatcher = UnityMainThreadDispatcherExtensions.Instance;

            await Task.Run(async () =>
            {
                foreach (var faceRotation in rotations)
                {
                    await dispatcher.InvokeAsync(() => FaceRotated?.Invoke(faceRotation));
                    await Task.Delay(750);
                }
            });
        }
    }
}

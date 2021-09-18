using MainThreadDispatcher;
using MainThreadDispatcher.Unity;
using Model;
using RubikVisualizers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventBus;
using EventBus.Events;
using UnityEngine;
using Zenject;

namespace Parser
{
    public class NotificationParser : INotificationParser
    {
        [Inject] private static IEventBus _eventBus;

        public void ParseNotification(byte[] notification, short dataSize)
        {
            var messageType = notification[2];

            switch (messageType)
            {
                case 1:
                    ParseFaceRotation(notification);
                    break;
                case 2:
                    ParseState(notification);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(notification), "notification type unknown");
            }
        }

        private void ParseFaceRotation(byte[] notification)
        {
            if (notification[1] == 8)
            {
                switch (notification[3])
                {
                    case 0:
                        InvokeFaceRotatedEvent(FaceRotation.M2);
                        break;
                    case 1:
                        InvokeFaceRotatedEvent(FaceRotation.M2Prime);
                        break;
                    case 4:
                        InvokeFaceRotatedEvent(FaceRotation.M3);
                        break;
                    case 5:
                        InvokeFaceRotatedEvent(FaceRotation.M3Prime);
                        break;
                    case 8:
                        InvokeFaceRotatedEvent(FaceRotation.M);
                        break;
                    case 9:
                        InvokeFaceRotatedEvent(FaceRotation.MPrime);
                        break;
                }
            }
            else
            {
                switch (notification[3])
                {
                    case 0:
                        InvokeFaceRotatedEvent(FaceRotation.B);
                        break;
                    case 1:
                        InvokeFaceRotatedEvent(FaceRotation.BPrime);
                        break;
                    case 2:
                        InvokeFaceRotatedEvent(FaceRotation.F);
                        break;
                    case 3:
                        InvokeFaceRotatedEvent(FaceRotation.FPrime);
                        break;
                    case 4:
                        InvokeFaceRotatedEvent(FaceRotation.U);
                        break;
                    case 5:
                        InvokeFaceRotatedEvent(FaceRotation.UPrime);
                        break;
                    case 6:
                        InvokeFaceRotatedEvent(FaceRotation.D);
                        break;
                    case 7:
                        InvokeFaceRotatedEvent(FaceRotation.DPrime);
                        break;
                    case 8:
                        InvokeFaceRotatedEvent(FaceRotation.R);
                        break;
                    case 9:
                        InvokeFaceRotatedEvent(FaceRotation.RPrime);
                        break;
                    case 10:
                        InvokeFaceRotatedEvent(FaceRotation.L);
                        break;
                    case 11:
                        InvokeFaceRotatedEvent(FaceRotation.LPrime);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(notification), "rotation unknown");
                }
            }
        }

        private static void InvokeFaceRotatedEvent(FaceRotation rotation) 
            => _eventBus.Invoke(new FaceRotated{Rotation = rotation});

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

        private void ParseState(byte[] notification)
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

            faces[0].Rotate(Rotation.One);
            faces[1].Rotate(Rotation.Two);
            faces[2].Rotate(Rotation.Two);
            faces[3].Rotate(Rotation.Two);
            faces[4].Rotate(Rotation.Two);
            faces[5].Rotate(Rotation.Prime);

            Debug.Log("State parsed");
            _eventBus.Invoke(new StateParsed{Faces = faces});
        }

        public async Task AnimateRotations(IEnumerable<FaceRotation> rotations)
        {
            IMainThreadDispatcher dispatcher = UnityMainThreadDispatcherExtensions.Instance;

            await Task.Run(async () =>
            {
                foreach (var faceRotation in rotations)
                {
                    await dispatcher.InvokeAsync(() => InvokeFaceRotatedEvent(faceRotation));
                    await Task.Delay(750);
                }
            });
        }
    }
}

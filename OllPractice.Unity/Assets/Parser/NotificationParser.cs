using EventBus;
using EventBus.Events;
using MainThreadDispatcher;
using MainThreadDispatcher.Unity;
using Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Parser
{
    public class NotificationParser : INotificationParser
    {
        [Inject] private IEventBus _eventBus;

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
                case 5:
                    ParseBattery(notification);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(notification), "notification type unknown");
            }
        }

        private void ParseBattery(byte[] notification)
        {
            var batteryLevel = notification[3] / 100f;
            _eventBus.Invoke(new BatteryLevelParsed(batteryLevel));
        }

        private void ParseFaceRotation(byte[] notification)
        {
            if (notification[1] == 8)
            {
                switch (notification[3])
                {
                    case 0:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.M2, Rotation.One));
                        break;
                    case 1:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.M2, Rotation.Prime));
                        break;
                    case 4:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.M3, Rotation.One));
                        break;
                    case 5:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.M3, Rotation.Prime));
                        break;
                    case 8:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.M, Rotation.One));
                        break;
                    case 9:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.M, Rotation.Prime));
                        break;
                }
            }
            else
            {
                switch (notification[3])
                {
                    case 0:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.B, Rotation.One));
                        break;
                    case 1:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.B, Rotation.Prime));
                        break;
                    case 2:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.F, Rotation.One));
                        break;
                    case 3:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.F, Rotation.Prime));
                        break;
                    case 4:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.U, Rotation.One));
                        break;
                    case 5:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.U, Rotation.Prime));
                        break;
                    case 6:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.D, Rotation.One));
                        break;
                    case 7:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.D, Rotation.Prime));
                        break;
                    case 8:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.R, Rotation.One));
                        break;
                    case 9:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.R, Rotation.Prime));
                        break;
                    case 10:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.L, Rotation.One));
                        break;
                    case 11:
                        InvokeFaceRotatedEvent(new FaceRotation(BasicRotation.L, Rotation.Prime));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(notification), "rotation unknown");
                }
            }
        }

        private void InvokeFaceRotatedEvent(FaceRotation rotation)
            => _eventBus.Invoke(new FaceRotated { Rotation = rotation });

        private static RubikColor ParseNotificationColor(byte col) =>
            col switch
            {
                0 => RubikColor.B,
                1 => RubikColor.G,
                2 => RubikColor.W,
                3 => RubikColor.Y,
                4 => RubikColor.R,
                5 => RubikColor.O,
                _ => throw new ArgumentOutOfRangeException(nameof(col), "color unknown")
            };

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
            _eventBus.Invoke(new StateParsed { Faces = faces });
        }

        public async Task AnimateRotations(IEnumerable<FaceRotation> rotations)
        {
            IMainThreadDispatcher dispatcher = UnityMainThreadDispatcherExtensions.Instance;

            await Task.Run(async () =>
            {
                foreach (var faceRotation in rotations)
                {
                    if (faceRotation.RotationType == Rotation.Two)
                    {
                        var oneRotation = new FaceRotation(faceRotation.BasicRotation, Rotation.One);
                        await dispatcher.InvokeAsync(() => InvokeFaceRotatedEvent(oneRotation));
                        await Task.Delay(750); 
                        await dispatcher.InvokeAsync(() => InvokeFaceRotatedEvent(oneRotation));
                        await Task.Delay(750);
                    }
                    else
                    {
                        await dispatcher.InvokeAsync(() => InvokeFaceRotatedEvent(faceRotation));
                        await Task.Delay(750);
                    }
                }
            });
        }
    }
}

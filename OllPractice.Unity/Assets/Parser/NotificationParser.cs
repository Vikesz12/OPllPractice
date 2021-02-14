using System;
using UnityEngine;

namespace Parser
{
    public class NotificationParser
    {
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

        private static string ParseFaceRotation(byte[] notification)
        {
            switch (notification[3])
            {
                case 0:
                    return "B";
                case 1:
                    return "B'";
                case 2:
                    return "F";
                case 3:
                    return "F'";
                case 4:
                    return "U";
                case 5:
                    return "U'";
                case 6:
                    return "D";
                case 7:
                    return "D'";
                case 8:
                    return "R";
                case 9:
                    return "R'";
                case 10:
                    return "L";
                case 11:
                    return "L'";
                default:
                    throw new ArgumentOutOfRangeException(nameof(notification), "rotation unknown");
            }
        }

        private static string ParseState(byte[] notification)
        {
            var startIndex = 4;

            for (var i = 0; i < 6; i++)
            {
                var face = notification[startIndex + i * 9];
                
            }
        }
    }
}

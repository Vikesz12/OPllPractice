using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using RubikVisualizers;

namespace Parser
{
    public interface INotificationParser
    {
        void ParseNotification(byte[] notification, short dataSize);
        event Action<FaceRotation> FaceRotated;
        event Action<Face[]> StateParsed;
        Task AnimateRotations(IEnumerable<FaceRotation> rotations);
    }
}
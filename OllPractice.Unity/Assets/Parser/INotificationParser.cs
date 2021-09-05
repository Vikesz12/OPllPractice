using RubikVisualizers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parser
{
    public interface INotificationParser
    {
        void ParseNotification(byte[] notification, short dataSize);
        Task AnimateRotations(IEnumerable<FaceRotation> rotations);
    }
}
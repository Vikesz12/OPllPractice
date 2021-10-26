using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using EventBus;
using EventBus.Events;
using Mirror;
using Model;
using RubikVisualizers;
using Zenject;

namespace Online
{
    public class RubikOnlineMessageSender : NetworkBehaviour
    {
        [Inject] private readonly IEventBus _eventBus;
        private RubikVisualizer _visualizer;

        private void Start()
        {
            _eventBus.Subscribe<StateParsed>(parsed => CmdStateParsed(parsed.Faces.Select(f => f.Squares).ToArray()));
            _eventBus.Subscribe<FaceRotated>(CmdFaceRotated);
            Init();
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<StateParsed>(parsed => CmdStateParsed(parsed.Faces.Select(f => f.Squares).ToArray()));
            _eventBus.Unsubscribe<FaceRotated>(CmdFaceRotated);
        }

        [Command]
        private void Init() =>
            _visualizer = isLocalPlayer ? 
                gameObject.GetComponent<RubikHolder>().GetCurrentVisualizer() : 
                gameObject.GetComponent<RubikHolderOnline>().GetCurrentVisualizer();

        [Command]
        private void CmdFaceRotated(FaceRotated obj) => _visualizer.OnFaceRotated(obj);

        [Command]
        private void CmdStateParsed(ulong[] faces)
        {
            var faceArray = faces.Select(f => new Face(f)).ToArray();
            _visualizer.LoadState(faceArray);
        }
    }
}

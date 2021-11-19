using EventBus;
using EventBus.Events;
using Mirror;
using Model;
using RubikVisualizers;
using System.Linq;
using Zenject;

namespace Online
{
    public class RubikOnlineMessageSender : NetworkBehaviour
    {
        [Inject] private readonly IEventBus _eventBus;

        private RubikHolder _holder;

        private void Awake() => _holder = gameObject.GetComponent<RubikHolder>();

        private void Start()
        {
            if (!isLocalPlayer && !isServer)
            {
                CmdGetState();
            }
            if(!isLocalPlayer || isServer) return;
            _eventBus.Subscribe<StateParsed>(parsed => CmdStateParsed(parsed.Faces.Select(f => f.Squares).ToArray()));
            _eventBus.Subscribe<FaceRotated>(rotated =>
            {
                var enumInts = new int[5];
                enumInts[0] = (int)rotated.Rotation.BasicRotation;
                enumInts[1] = (int)rotated.Rotation.TurnType;
                enumInts[2] = (int)rotated.Rotation.RotationType;
                enumInts[3] = (int)rotated.Rotation.CubeRotation;
                enumInts[4] = (int)rotated.Rotation.DoubleLayerRotation;
                CmdFaceRotated(enumInts);
            });
        }

        

        public override void OnStartClient()
        {
            base.OnStartClient();
            if(isClient) return;
            RpcStateParsed(_holder.GetCurrentVisualizer().GetCube.GetFaces.Select(f => f.Squares).ToArray());
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<StateParsed>(parsed => CmdStateParsed(parsed.Faces.Select(f => f.Squares).ToArray()));
            _eventBus.Unsubscribe<FaceRotated>(rotated =>
            {
                var enumInts = new int[5];
                enumInts[0] = (int)rotated.Rotation.BasicRotation;
                enumInts[1] = (int)rotated.Rotation.TurnType;
                enumInts[2] = (int)rotated.Rotation.RotationType;
                enumInts[3] = (int)rotated.Rotation.CubeRotation;
                enumInts[4] = (int)rotated.Rotation.DoubleLayerRotation;
                CmdFaceRotated(enumInts);
            });
        }

        [Command]
        private void CmdFaceRotated(int[] enums)
        {
            var faceRotation = new FaceRotation(enums);
            _holder.GetCurrentVisualizer().OnFaceRotated(new FaceRotated(faceRotation));
            RpcFaceRotated(enums);
        }

        [ClientRpc(includeOwner = false)]
        private void RpcFaceRotated(int[] enumInts)
        {
            if(isServer) return;
            var obj = new FaceRotation(enumInts);
            _holder.GetCurrentVisualizer().OnFaceRotated(new FaceRotated(obj));
        }

        [Command]
        private void CmdStateParsed(ulong[] faces)
        {
            var faceArray = faces.Select(f => new Face(f)).ToArray();
            _holder.LoadState(faceArray);
            RpcStateParsed(faces);
        }

        [ClientRpc(includeOwner = false)]
        private void RpcStateParsed(ulong[] faces)
        {
            if(isServer) return;
            var faceArray = faces.Select(f => new Face(f)).ToArray();
            _holder.LoadState(faceArray);
        }

        [Command(requiresAuthority = false)]
        private void CmdGetState()
        {
            var faces = _holder.GetCurrentVisualizer().GetCube.GetFaces.Select(f => f.Squares).ToArray();
            RpcSetState(faces);
        }

        [ClientRpc]
        private void RpcSetState(ulong[] faces)
        {
            if(isLocalPlayer || isServer) return;
            var faceArray = faces.Select(f => new Face(f)).ToArray();
            _holder.LoadState(faceArray);
        }
    }
}

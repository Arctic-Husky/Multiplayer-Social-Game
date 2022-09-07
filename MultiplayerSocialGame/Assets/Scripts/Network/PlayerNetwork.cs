using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<PlayerNetworkData> _netState = new NetworkVariable<PlayerNetworkData>(writePerm: NetworkVariableWritePermission.Owner);

    private Vector3 _vel;
    private float _rotVel;

    //[SerializeField] private float _cheapInterpolationTime = 0.1f;

    private void Update()
    {
        if(IsOwner)
        {
            _netState.Value = new PlayerNetworkData()
            {
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles
            };
        }
        else
        {
            /*transform.position = Vector3.SmoothDamp(transform.position, _netState.Value.Position, ref _vel, _cheapInterpolationTime);
            transform.rotation = Quaternion.Euler(
                Mathf.SmoothDampAngle(transform.rotation.eulerAngles.x, _netState.Value.Rotation.x, ref _rotVel, _cheapInterpolationTime),
                Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, _netState.Value.Rotation.y, ref _rotVel, _cheapInterpolationTime),
                Mathf.SmoothDampAngle(transform.rotation.eulerAngles.z, _netState.Value.Rotation.z, ref _rotVel, _cheapInterpolationTime)
                );*/

            transform.position = _netState.Value.Position;
            transform.rotation = Quaternion.Euler(_netState.Value.Rotation.x, _netState.Value.Rotation.y, _netState.Value.Rotation.z);
        }
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private float _x, _y, _z;
        private float _xRot, _yRot, _zRot;

        internal Vector3 Position
        {
            get => new Vector3(_x, _y, _z);
            set
            {
                _x = value.x;
                _y = value.y;
                _z = value.z;
            }
        }

        internal Vector3 Rotation
        {
            get => new Vector3(_xRot, _yRot, _zRot);
            set
            {
                _xRot = value.x;
                _yRot = value.y;
                _zRot = value.z;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T: IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);
            serializer.SerializeValue(ref _z);

            serializer.SerializeValue(ref _xRot);
            serializer.SerializeValue(ref _yRot);
            serializer.SerializeValue(ref _zRot);
        }
    }
}

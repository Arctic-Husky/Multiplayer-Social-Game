using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // acao ao spawnar se for o dono do objeto
        }
    }



    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

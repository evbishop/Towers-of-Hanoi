using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : NetworkBehaviour
{
    [SyncVar]
    Player owner;

    public override void OnStartServer()
    {
        owner = connectionToClient.identity.GetComponent<Player>();
    }

    void Update()
    {
        if (!transform.parent) transform.parent = owner.RingsParent;
    }
}

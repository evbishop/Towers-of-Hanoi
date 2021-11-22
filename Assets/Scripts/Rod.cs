using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rod : NetworkBehaviour
{
    [SerializeField] Transform[] ringPoints;
    [SerializeField] Transform bufferRingPoint;
    [SerializeField] bool finalRod;
    
    [SyncVar]
    Player owner;

    public SyncList<GameObject> Rings { get; } = new SyncList<GameObject>();

    public override void OnStartServer()
    {
        owner = connectionToClient.identity.GetComponent<Player>();
    }

    void Update()
    {
        if (!transform.parent) transform.parent = owner.RodsParent;
    }

    void OnMouseDown()
    {
        if (!owner.BufferRing)
        {
            if (Rings.Count == 0) return;
            TakeRing();
        }
        else if (Rings.Count == 0
            || owner.BufferRing.transform.localScale.x
            < Rings[Rings.Count - 1].transform.localScale.x)
            PlaceRing();
    }

    [Command]
    public void TakeRing()
    {
        owner.BufferRing = Rings[Rings.Count - 1];
        owner.BufferRing.transform.position = bufferRingPoint.position;
        if (finalRod) owner.RingsOnTarget--;
        Rings.Remove(owner.BufferRing);
    }


    [Command]
    public void PlaceRing()
    {
        owner.BufferRing.transform.position = ringPoints[Rings.Count].position;
        Rings.Add(owner.BufferRing);
        owner.BufferRing = null;
        if (finalRod) owner.RingsOnTarget++;
    }
}

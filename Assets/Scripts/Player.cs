using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : NetworkBehaviour
{
    [SerializeField] TextMesh ringsOnTargetText;
    [SerializeField] TMP_Text gameOverText;
    [SerializeField] Transform ringsParent, rodsParent;
    [SerializeField] GameObject[] ringPrefabs, rodPrefabs;
    
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;
    public static event Action ClientOnInfoUpdated;

    public Transform RingsParent { get { return ringsParent; } }
    public Transform RodsParent { get { return rodsParent; } }

    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))] string displayName;
    public string DisplayName 
    { 
        get { return displayName; }
        set { displayName = value; }
    }

    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerUpdated))] bool partyOwner;
    public bool PartyOwner
    {
        get { return partyOwner; }
        set { partyOwner = value; }
    }

    [SyncVar]
    int ringsOnTarget;
    public int RingsOnTarget
    {
        get { return ringsOnTarget; }
        set
        {
            ringsOnTarget = value;
            ClientSetRingsOnTargetText(ringsOnTarget.ToString());
            if (ringsOnTarget == 8) GameOver("Победа!");
        }
    }

    [SyncVar]
    GameObject bufferRing;
    public GameObject BufferRing 
    { 
        get { return bufferRing; } 
        set { bufferRing = value; }
    }

    #region Server
    public override void OnStartServer()
    {
        Rod startRod = null;
        foreach (var rodPrefab in rodPrefabs)
        {
            var rodInstance = Instantiate(rodPrefab, rodsParent);
            if (rodInstance.CompareTag("Start")) 
                startRod = rodInstance.GetComponent<Rod>();
            NetworkServer.Spawn(rodInstance, connectionToClient);
        }
        foreach (var ringPrefab in ringPrefabs)
        {
            var ringInstance = Instantiate(ringPrefab, ringsParent);
            startRod.Rings.Add(ringInstance);
            NetworkServer.Spawn(ringInstance, connectionToClient);
        }
        DontDestroyOnLoad(gameObject);
    }

    [Command]
    public void CmdStartGame()
    {
        if (!PartyOwner) return;
        ((HanoiNetworkManager)NetworkManager.singleton).StartGame();
    }
    #endregion

    #region Client
    public override void OnStartClient()
    {
        if (NetworkServer.active) return;
        ((HanoiNetworkManager)NetworkManager.singleton).Players.Add(this);
        DontDestroyOnLoad(gameObject);
    }

    public override void OnStopClient()
    {
        ClientOnInfoUpdated?.Invoke();
        if (!isClientOnly) return;
        ((HanoiNetworkManager)NetworkManager.singleton).Players.Remove(this);
    }

    [ClientRpc]
    void ClientSetRingsOnTargetText(string text)
    {
        ringsOnTargetText.text = text;
    }

    [ClientRpc]
    void GameOver(string message)
    {
        gameOverText.text = message;
    }

    void ClientHandleDisplayNameUpdated(string old, string newDisplayName)
    {
        ClientOnInfoUpdated?.Invoke();
    }

    void AuthorityHandlePartyOwnerUpdated(bool oldState, bool newState)
    {
        if (!hasAuthority) return;
        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }
    #endregion
}

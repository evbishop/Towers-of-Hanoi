using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HanoiNetworkManager : NetworkManager
{
    public List<Player> Players { get; } = new List<Player>();
    bool gameInProgress;

    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    #region Server
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (gameInProgress) conn.Disconnect();
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        var player = conn.identity.GetComponent<Player>();
        Players.Remove(player);
        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        Players.Clear();
        gameInProgress = false;
    }

    public void StartGame()
    {
        if (Players.Count < 2) return;
        gameInProgress = true;
        ServerChangeScene("Game Scene");
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Quaternion rotation = Quaternion.Euler(numPlayers == 0
            ? Vector3.zero
            : new Vector3(0, 180, 0));
        GameObject playerInstance = Instantiate(playerPrefab, Vector3.zero, rotation);
        NetworkServer.AddPlayerForConnection(conn, playerInstance);
        var player = playerInstance.GetComponent<Player>();
        Players.Add(player);
        player.DisplayName = $"Player {Players.Count}";
        player.PartyOwner = Players.Count == 1;
    }
    #endregion

    #region Client
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        ClientOnDisconnected?.Invoke();
        if (Players.Count == 0) SceneManager.LoadScene(0);
    }

    public override void OnStopClient()
    {
        Players.Clear();
    }
    #endregion
}

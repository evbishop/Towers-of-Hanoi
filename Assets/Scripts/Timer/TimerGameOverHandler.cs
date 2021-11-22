using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerGameOverHandler : NetworkBehaviour
{
    public override void OnStartServer()
    {
        TimerGame.ServerOnTimeRanOut += HandleTimeRanOut;
        DontDestroyOnLoad(gameObject);
    }

    public override void OnStopServer()
    {
        TimerGame.ServerOnTimeRanOut -= HandleTimeRanOut;
    }

    void HandleTimeRanOut()
    {
        var players = ((HanoiNetworkManager)NetworkManager.singleton).Players;
        Player winner = null;
        int mostRingsOnTarget = 0;
        foreach (var player in players)
            if (player.RingsOnTarget > mostRingsOnTarget)
            {
                mostRingsOnTarget = player.RingsOnTarget;
                winner = player;
            }
        foreach (var player in players)
            player.GameOver(
                winner ? $"{winner.DisplayName}\nпобедил(а)!"
                : "Ничья!");
    }
}

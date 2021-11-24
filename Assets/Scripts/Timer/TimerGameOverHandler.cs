using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerGameOverHandler : NetworkBehaviour
{
    public override void OnStartServer()
    {
        TimerCounter.ServerOnTimeRanOut += ServerHandleTimeRanOut;
    }

    public override void OnStopServer()
    {
        TimerCounter.ServerOnTimeRanOut -= ServerHandleTimeRanOut;
    }

    void ServerHandleTimeRanOut()
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

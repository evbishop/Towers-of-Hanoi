using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerGameOverHandler : NetworkBehaviour
{
    public override void OnStartServer()
    {
        TimerCounter.ServerOnTimeRanOut += ServerHandleTimeRanOut;
        DontDestroyOnLoad(gameObject);
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
        bool draw = false;
        foreach (var player in players)
            if (player.RingsOnTarget > mostRingsOnTarget)
            {
                mostRingsOnTarget = player.RingsOnTarget;
                winner = player;
                draw = false;
            }
            else if (player.RingsOnTarget == mostRingsOnTarget && mostRingsOnTarget != 0)
                draw = true;
        foreach (var player in players)
            if (draw) player.GameOver("Ничья!");
            else player.GameOver(
                winner ? $"{winner.DisplayName}\nпобедил(а)!" : "Ничья!");
    }
}

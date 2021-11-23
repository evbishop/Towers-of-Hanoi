using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerCounter : NetworkBehaviour
{
    [SyncVar]
    int timer;

    public override void OnStartServer()
    {
        TimerSelector.ServerOnTimerChanged += HandleTimerChanged;
    }

    public override void OnStopServer()
    {
        TimerSelector.ServerOnTimerChanged -= HandleTimerChanged;
    }

    void HandleTimerChanged(int value)
    {
        timer = value;
        print(timer);
    }
}

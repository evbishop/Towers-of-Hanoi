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
        DontDestroyOnLoad(gameObject);
        HanoiNetworkManager.ServerStartTimer += HandleStartTimer;
    }

    public override void OnStopServer()
    {
        TimerSelector.ServerOnTimerChanged -= HandleTimerChanged;
        HanoiNetworkManager.ServerStartTimer -= HandleStartTimer;
    }

    void HandleStartTimer()
    {
        if (timer > 0) StartCoroutine(TimerCoroutine());
    }

    void HandleTimerChanged(int value)
    {
        timer = value;
    }

    IEnumerator TimerCoroutine()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            print(timer);
        }
    }
}

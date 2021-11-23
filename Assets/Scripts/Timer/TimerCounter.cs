using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerCounter : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleTimerUpdated))]
    int timer;

    public static event Action<int> ClientOnTimerUpdated;

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
        if (timer > 0) StartCoroutine(CountdownCoroutine());
    }

    void HandleTimerChanged(int value)
    {
        timer = value;
    }

    IEnumerator CountdownCoroutine()
    {
        ClientOnTimerUpdated?.Invoke(timer);
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
        }
    }

    void HandleTimerUpdated(int oldTime, int newTime)
    {
        ClientOnTimerUpdated?.Invoke(newTime);
    }
}

using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerGame : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleTimerUpdated))]
    int timer;

    public static event Action<int> ClientOnTimerUpdated;
    public static event Action ServerOnTimeRanOut;

    public override void OnStartServer()
    {
        TimerSelector.ServerOnTimerChanged += HandleTimerChanged;
        HanoiNetworkManager.StartTimer += HandleStartTimer;
        DontDestroyOnLoad(gameObject);
    }

    public override void OnStopServer()
    {
        TimerSelector.ServerOnTimerChanged -= HandleTimerChanged;
        HanoiNetworkManager.StartTimer -= HandleStartTimer;
    }

    void HandleStartTimer()
    {
        if (timer > 0) StartCoroutine(TimerCoroutine());
    }

    void HandleTimerChanged(int value)
    {
        timer = value;
    }

    void HandleTimerUpdated(int oldTime, int newTime)
    {
        ClientOnTimerUpdated?.Invoke(newTime);
    }

    IEnumerator TimerCoroutine()
    {
        if (timer == 0) yield break;
        ClientOnTimerUpdated?.Invoke(timer);
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
        }
        ServerOnTimeRanOut?.Invoke();
    }
}

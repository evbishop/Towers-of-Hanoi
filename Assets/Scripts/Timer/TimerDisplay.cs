using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] Text timeText;

    void Start()
    {
        TimerGame.ClientOnTimerUpdated += ClientHandleTimerUpdated;
    }

    void OnDestroy()
    {
        TimerGame.ClientOnTimerUpdated -= ClientHandleTimerUpdated;
    }

    void ClientHandleTimerUpdated(int time)
    {
        timeText.text = FormatTime(time);
    }

    string FormatTime(int time)
    {
        TimeSpan result = TimeSpan.FromSeconds(time);
        return result.ToString("mm':'ss");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;

    void Start()
    {
        TimerCounter.ClientOnTimerUpdated += ClientHandleTimerUpdated;
    }

    void OnDestroy()
    {
        TimerCounter.ClientOnTimerUpdated -= ClientHandleTimerUpdated;
    }

    void ClientHandleTimerUpdated(int time)
    {
        timeText.text = TimeSpan.FromSeconds(time).ToString("mm':'ss");
    }
}

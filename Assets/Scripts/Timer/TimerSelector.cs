using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerSelector : MonoBehaviour
{
    [SerializeField] Dropdown dropdown;
    [SerializeField] List<int> timersInSeconds;

    public static event Action<int> ServerOnTimerChanged;

    public void ChangeTimer()
    {
        ServerOnTimerChanged?.Invoke(timersInSeconds[dropdown.value]);
    }
}

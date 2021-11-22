using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class ButtonOffline : MonoBehaviour
{
    public void GoOffline()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
            NetworkManager.singleton.StopHost();
        else NetworkManager.singleton.StopClient();
    }
}

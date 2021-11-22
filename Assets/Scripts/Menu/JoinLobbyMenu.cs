using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] GameObject landingPagePanel;
    [SerializeField] InputField addressInput;
    [SerializeField] Button joinButton;

    void OnEnable()
    {
        HanoiNetworkManager.ClientOnConnected += HandleClientConnected;
        HanoiNetworkManager.ClientOnDisconnected += HandleClientDisconnected;
    }

    void OnDisable()
    {
        HanoiNetworkManager.ClientOnConnected -= HandleClientConnected;
        HanoiNetworkManager.ClientOnDisconnected -= HandleClientDisconnected;
    }

    public void Join()
    {
        string address = addressInput.text;
        NetworkManager.singleton.networkAddress = address;
        NetworkManager.singleton.StartClient();
        joinButton.interactable = false;
    }

    void HandleClientConnected()
    {
        joinButton.interactable = true;
        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}

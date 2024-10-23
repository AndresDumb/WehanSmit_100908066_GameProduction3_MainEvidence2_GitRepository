using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager = null;
    [SerializeField] private GameObject _Canvas = null;
    [SerializeField] private GameObject landingPage = null;
    [SerializeField] private TMP_InputField ipAddressField = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable()
    {
        NetworkManager.OnClientConnected += HandleClientConnected;
        NetworkManager.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkManager.OnClientConnected -= HandleClientConnected;
        NetworkManager.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        string ipAddress = ipAddressField.text;
        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;
        gameObject.SetActive(false);
        landingPage.SetActive(false);
        _Canvas.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}

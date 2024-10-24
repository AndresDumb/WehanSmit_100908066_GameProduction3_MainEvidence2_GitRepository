using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager = null;
    [SerializeField] private GameObject landingPage = null;

    public void HostLobby()
    {
        
        networkManager.StartHost();
        landingPage.SetActive(false);
    }
}

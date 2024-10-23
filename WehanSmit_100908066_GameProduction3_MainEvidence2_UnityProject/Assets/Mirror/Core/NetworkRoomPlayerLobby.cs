using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;


public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    public bool isLeader = false;
    [SerializeField] private GameObject LobbyUI = null;
    [SerializeField] private Text[] playerNames = new Text[4];
    [SerializeField] private Button StartButton = null;
    
    

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    public string Display = null;
    
    

    public bool IsLeader
    {
        set
        {
            isLeader = value;
            StartButton.gameObject.SetActive(value);
        }
    }

    private NetworkManager room;

    private NetworkManager Room
    {
        get
        {
            if (room != null)
            {
                return room;
            }

            return room = NetworkManager.singleton;


        }
    }

    public override void OnStartAuthority()
    {
        DisplayName = PlayerPrefs.GetString("PlayerName");
        
        CmdSetDisplayName(DisplayName);
        LobbyUI.SetActive(true);
        StartButton.interactable = true;
    }

    [Command]
    private void CmdSetDisplayName(string display)
    {
        DisplayName = display;
    }

    public override void OnStartClient()
    {
        Room.roomPlayers.Add(this);
        
        UpdateDisplay();
    }

    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    private void UpdateDisplay()
    {
        if (!isLocalPlayer)
        {


            foreach (var player in Room.roomPlayers)
            {
                if (player.isLocalPlayer)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }

        for (int i = 0; i < playerNames.Length; i++)
        {
            playerNames[i].text = "Waiting For Player...";
            
        }

        for (int i = 0; i < Room.roomPlayers.Count; i++)
        {
            playerNames[i].text = Room.roomPlayers[i].DisplayName;
        }
    }

    

    [Command]
    public void CmdStartgame()
    {
        if (Room.roomPlayers[0].connectionToClient != connectionToClient)
        {
            return;
        }
        Room.StartGame();
    }
}
    


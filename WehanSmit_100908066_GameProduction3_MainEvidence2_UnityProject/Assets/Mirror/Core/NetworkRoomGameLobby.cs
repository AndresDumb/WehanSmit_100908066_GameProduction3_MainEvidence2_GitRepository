using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;


public class NetworkRoomGameLobby : NetworkBehaviour
{
    
    
    

    [SyncVar]
    private string displayName = "Loading...";
    
    
    

    

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
    
    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        Room.GamePlayers.Add(this);
       
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }
    [Server]
    public void SetDisplay()
    {
        this.displayName = PlayerPrefs.GetString("PlayerName");
    }
    
    

    
}
    


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
    }

    

    
    [Server]
    public void SetDisplay(string Display)
    {
        this.displayName = Display;
    }
    
    

    
}
    


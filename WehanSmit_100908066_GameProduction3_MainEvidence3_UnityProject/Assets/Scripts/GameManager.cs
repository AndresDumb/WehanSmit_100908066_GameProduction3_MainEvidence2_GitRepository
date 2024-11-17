using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public List<GameObject> Players = new List<GameObject>();
    

    public GameObject EndScreen;
    private string EndString = "GAME OVER";
    public TMP_Text EndText;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
    public void FlagReached()
    {
        
        EndText.text = EndString;
        EndScreen.SetActive(true);
        foreach (var Player in Players)
        {
            Player.GetComponent<PlayerMovement>().Paused = true;
        }
    }
}

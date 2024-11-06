using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> Players = new List<GameObject>();

    public GameObject EndScreen;
    private string EndString = " HAS WON";
    public TMP_Text EndText;

    public Button QuitButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var Player in Players)
        {
            
            if (Player.GetComponent<PlayerMovement>().FlagReached)
            {
                FlagReached(Player.GetComponent<PlayerMovement>().Nametext);
            }
        }
    }
    
    public void FlagReached(string ID)
    {
        EndString = ID.ToUpper() + EndString;
        EndText.text = EndString;
        EndScreen.SetActive(true);
    }
}

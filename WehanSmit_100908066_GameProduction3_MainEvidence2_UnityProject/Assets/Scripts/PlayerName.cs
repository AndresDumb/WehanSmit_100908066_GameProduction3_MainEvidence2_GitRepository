using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput = null;

    [SerializeField] private Button continueButton = null;
    
    
    public static string Display { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";

    
    // Start is called before the first frame update
    void Start()
    {
        SetUpInput();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetUpInput()
    {
        if (!PlayerPrefs.HasKey((PlayerPrefsNameKey)))
        {
            return;
            
        }

        string defName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        nameInput.text = defName;
        SetPlayerName(defName);
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        Display = nameInput.text;
        PlayerPrefs.SetString(PlayerPrefsNameKey, Display);
    }
}

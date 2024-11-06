using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class SteamLobby : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] public GameObject Panel = null;
    [SerializeField] public GameObject ButtonPrivate = null;
    [SerializeField] public GameObject ButtonPublic = null;
    [SerializeField] public GameObject ButtonFriends = null;
    [SerializeField] public GameObject NetIssue = null;
    [SerializeField] public GameObject Cam = null;
    private GameObject Button = null;
    
    private NetworkManager NetManager;
    

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequested;
    protected Callback<LobbyEnter_t> LobbyEnter;

    private const string HostAddressKey = "HostAddress";
    
    // Start is called before the first frame update
    void Start()
    {
        NetManager = GetComponent<NetworkManager>();
        

        if (!SteamManager.Initialized)
        {
            NetIssue.SetActive(true);
            return;
        }
        NetIssue.SetActive(false);
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequested);
        LobbyEnter = Callback<LobbyEnter_t>.Create(LobbyEntered);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            ButtonPublic.SetActive(true);
            ButtonPrivate.SetActive(true);
            ButtonFriends.SetActive(true);
            return;
        }
        
        NetManager.StartHost();
        NetManager.playerPrefab.GetComponent<PlayerMovement>().isHost = true;
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey,
            SteamUser.GetSteamID().ToString());
    }

    private void OnJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void LobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active)
        {
            return;
        }

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

        NetManager.networkAddress = hostAddress;
        NetManager.StartClient();
        
        ButtonPublic.SetActive(false);
        ButtonPrivate.SetActive(false);
        ButtonFriends.SetActive(false);
        Panel.SetActive(false);
        Cam.SetActive(false);
    }

    public void CreatePublicLobby()
    {
        ButtonPublic.SetActive(false);
        ButtonPrivate.SetActive(false);
        ButtonFriends.SetActive(false);
        Panel.SetActive(false);
        Cam.SetActive(false);
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, NetManager.maxConnections);
    }

    public void CreatePrivateLobby()
    {
        ButtonPublic.SetActive(false);
        ButtonPrivate.SetActive(false);
        ButtonFriends.SetActive(false);
        Panel.SetActive(false);
        Cam.SetActive(false);
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePrivate, NetManager.maxConnections);
    }

    public void CreateFriendsOnlyLobby()
    {
        ButtonPublic.SetActive(false);
        ButtonPrivate.SetActive(false);
        ButtonFriends.SetActive(false);
        Panel.SetActive(false);
        Cam.SetActive(false);
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, NetManager.maxConnections);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

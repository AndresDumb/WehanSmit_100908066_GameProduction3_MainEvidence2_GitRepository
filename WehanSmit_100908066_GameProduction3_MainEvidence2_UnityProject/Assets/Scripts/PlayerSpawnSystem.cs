using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;
    private static List<float> XSpawnPos = new List<float>();
    private int Index = 0;

    public void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            XSpawnPos.Add(-6f + (4f * i));
        }
    }

    public override void OnStartServer() => NetworkManager.OnServerReadied += SpawnPlayer;
    
    [ServerCallback]
    public void OnDestroy()
    {
        NetworkManager.OnServerReadied -= SpawnPlayer;
    }

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        XSpawnPos = new List<float>();
        for (int i = 0; i < 4; i++)
        {
            XSpawnPos.Add(-6f + (4f * i));
        }
        Vector3 Pos = new Vector3(XSpawnPos[Index], -1f, 0f);
        GameObject playerInstance = Instantiate(playerPrefab);
        playerInstance.transform.position = Pos;
        playerInstance.transform.rotation = Quaternion.identity;
        NetworkServer.Spawn(playerInstance,conn);
        Index++;
    }
}

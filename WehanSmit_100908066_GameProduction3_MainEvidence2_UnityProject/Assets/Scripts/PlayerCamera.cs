using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Mirror;

public class PlayerCamera : NetworkBehaviour
{
    
    [SerializeField] private Camera VCamera = null;

    public override void OnStartAuthority()
    {
        
        VCamera.gameObject.SetActive(true);
        enabled = true;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAssign : NetworkBehaviour {

    [SyncVar]
    public int playerNo = 0;

    public GameObject[] weaponRespawnPoints = new GameObject[8];


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    [Command]
    public void CmdGetNum() {
        playerNo++;
    }

}

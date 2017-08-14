using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAssign : NetworkBehaviour {

    [SyncVar]
    public int playerNo = 0;

    public GameObject[] weaponRespawnPoints = new GameObject[8];
    //public GameObject[] playerRespawnPoints = new GameObject[4]; already had values

	// Use this for initialization
	void Start () {
       // this.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);//NOPE


    }
	
	// Update is called once per frame
	void Update () {
		
	}



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAssign : NetworkBehaviour {

    [SyncVar]
    public int playerNo = 0;

    public GameObject[] weaponRespawnPoints = new GameObject[28];

}

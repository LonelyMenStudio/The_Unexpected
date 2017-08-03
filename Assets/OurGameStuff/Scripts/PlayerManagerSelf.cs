using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManagerSelf : NetworkBehaviour {

    public GameObject[] playersLocalCopy = new GameObject[5];
    public List<GameObject> droppedWeaponsLocalCopy = new List<GameObject>();
    private PlayerManager syncListTo;
    private GameObject manager;

	// Use this for initialization
	void Start () {
        manager = GameObject.Find("Manager");
        syncListTo = manager.GetComponent<PlayerManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddSelf(GameObject sPlayer, int playerNum) {
        if (!isLocalPlayer) {
            return;
        }
        CmdAddToArray(sPlayer, playerNum);
    }

    [Command]
    void CmdAddToArray(GameObject obj, int num) {
        // this code is only executed on the server
        RpcAddToArray(obj, num); // invoke Rpc on all clients
    }


    [ClientRpc]
    void RpcAddToArray(GameObject obj, int num) {
        playersLocalCopy[num] = obj;
        syncPlayers();
    }
    
    public void AddWeaponToList(GameObject weapon) {
        if (!isLocalPlayer) {
            return;
        }
        CmdAddToList(weapon);
    }
    public void DropWeaponFromList(GameObject weapon) {
        if (!isLocalPlayer) {
            return;
        }
        CmdDropFromList(weapon);
    }
    [Command]
    void CmdAddToList(GameObject obj) {
        RpcAddToList(obj);
    }
    [ClientRpc]
    void RpcAddToList(GameObject obj) {
        droppedWeaponsLocalCopy.Add(obj);
    }

    [Command]
    void CmdDropFromList(GameObject obj) {
        RpcDropFromList(obj);
    }
    [ClientRpc]
    void RpcDropFromList(GameObject obj) {
        droppedWeaponsLocalCopy.Remove(obj);    }

    void syncPlayers() {
        for(int i = 1; i<5; i++)
        playersLocalCopy[i] = syncListTo.players[i];
    }
}

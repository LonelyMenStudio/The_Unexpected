using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkXYZSync : NetworkBehaviour {

    GameObject player;

    //[SyncVar]
    public Vector3 test;


	// Use this for initialization
	void Start () {
        player = this.transform.gameObject;
        
	}
	
	// Update is called once per frame
	void Update () {
        CmdSetL();
        if (!isServer) {
            CmdSyncXYZ(test);
        } else {
            RpcSyncXYZ(test);
        }
    }

    void CmdSetL() {
        test = player.transform.localPosition;
    }
    [Command]
    void CmdSyncXYZ(Vector3 i) {
        RpcSyncXYZ(i);
    }

    [ClientRpc]
    void RpcSyncXYZ(Vector3 i) {
        if (!isLocalPlayer) {
            player.transform.localPosition = i;
        }
    }

    /*



    [Command]
    void CmdSwitchWeapon(int weapon) {
        RpcSwitchWeapon(weapon);
    }
    [ClientRpc]
    void RpcSwitchWeapon(int weapon) {
        if (weapon == 0) {
            weaponOut = 0;
    childMelee.SetActive(true);//***
            childWeapon1.SetActive(false);//***
            childWeapon2.SetActive(false);//***
        }
        if (weapon == 1) {
            weaponOut = 1;
            childMelee.SetActive(false);//***
            childWeapon1.SetActive(true);//***
            childWeapon2.SetActive(false);//***
        } else if (weapon == 2) {
            weaponOut = 2;
            childMelee.SetActive(false);//***
            childWeapon1.SetActive(false);//***
            childWeapon2.SetActive(true);//***
        }
    }
    */


}

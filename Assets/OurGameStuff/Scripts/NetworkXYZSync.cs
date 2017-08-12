using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkXYZSync : NetworkBehaviour {

    private GameObject player;
    private Vector3 test = new Vector3(0,0,0);
    private Vector3 teleportTo;
    private bool startTele = false;
    public Quaternion test2;

    //probs could change locals to non locals

	// Use this for initialization
	void Start () {
        player = this.transform.gameObject;

	}
	
    
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) {
            return;
        }
        if(startTele == true) {
            this.transform.position = teleportTo;
            if (!isServer) {
                CmdSyncXYZ2(teleportTo);
            } else {
                RpcSyncXYZ2(teleportTo);
            }
            startTele = false;
            return;
        }
        SetL();
        if (!isServer) {
            CmdSyncXYZ(test, test2);
        } else {
            RpcSyncXYZ(test, test2);
        }
    }

    public void Teleport(Vector3 whereTo) {//could change to gameObject
        Debug.Log("gets called");
        teleportTo = whereTo;
        startTele = true;
    }
    void SetL() {
        test = player.transform.localPosition;
        test2 = player.transform.localRotation;
    }
    [Command]
    void CmdSyncXYZ2(Vector3 i) {
        RpcSyncXYZ2(i);
    }

    [ClientRpc]
    void RpcSyncXYZ2(Vector3 i) {
        if (!isLocalPlayer) {
            player.transform.localPosition = i;
        }
    }
    //for inc rotation
    [Command]
    void CmdSyncXYZ(Vector3 i, Quaternion k) {
        RpcSyncXYZ(i, k);
    }

    [ClientRpc]
    void RpcSyncXYZ(Vector3 i, Quaternion k) {
        if (!isLocalPlayer) {
            player.transform.localPosition = i;
            player.transform.localRotation = k;
        }
    }


}

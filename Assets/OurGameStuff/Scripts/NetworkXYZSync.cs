using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkXYZSync : NetworkBehaviour {

    private GameObject player;
    private Vector3 playerLocation = new Vector3(0,0,0);
    private Vector3 teleportTo;
    public bool startTele = false;
    public GameObject childBodyRotation;
    //public Quaternion bodyRotation = new Quaternion(0,0,0,0); // Y W - Sync Both
    public GameObject childHeadRotation;
    //public Quaternion headRotation = new Quaternion(0,0,0,0); // X W - Sync X

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
                CmdSyncXYZTele(teleportTo);
            } else {
                RpcSyncXYZTele(teleportTo);
            }
            startTele = false;
            return;
        }
        SetL();
        if (!isServer) {
            CmdSyncXYZ(playerLocation);//, bodyRotation, headRotation);
        } else {
            RpcSyncXYZ(playerLocation); // , bodyRotation, headRotation);//might be the problem
        }
    }

    public void Teleport(Vector3 whereTo) {//could change to gameObject
        teleportTo = whereTo;
        startTele = true;
    }
    void SetL() {
        playerLocation = player.transform.localPosition;
        //bodyRotation = childBodyRotation.transform.rotation * new Quaternion(0, 90, 0, 0); 
        //bodyRotation.y = bodyRotation.y + 90;apparently broke everything
        //headRotation = childHeadRotation.transform.localRotation;
    }
    [Command]
    void CmdSyncXYZTele(Vector3 i) {
        RpcSyncXYZTele(i);
        Debug.Log("well then");
    }

    [ClientRpc]
    void RpcSyncXYZTele(Vector3 i) {
        if (!isLocalPlayer) {
            player.transform.localPosition = i;
        }
    }
    [Command]
    void CmdSyncXYZ(Vector3 i) {//, Quaternion body, Quaternion head) {
        RpcSyncXYZ(i);//, body, head);
    }

    [ClientRpc]
    void RpcSyncXYZ(Vector3 i) { //, Quaternion body, Quaternion head) { 
        if (!isLocalPlayer) {
            player.transform.localPosition = i;
            //player.transform.localRotation = body;
            //childHeadRotation.transform.localRotation = head;
            //childBodyRotation.transform.localRotation = body;
        }
    }
}

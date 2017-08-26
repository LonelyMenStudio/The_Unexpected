using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkXYZSync : NetworkBehaviour {

    private GameObject player;
    private Vector3 playerLocation = new Vector3(0, 0, 0);
    private Vector3 teleportTo;
    public bool startTele = false;

    // Use this for initialization
    void Start() {
        player = this.transform.gameObject;
    }


    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer) {
            return;
        }
        if (startTele == true) {
            this.transform.position = teleportTo;
            if (!isServer) {
                CmdSyncXYZTele(teleportTo, player);
            } else {
                RpcSyncXYZTele(teleportTo, player);
            }
            startTele = false;
            return;
        }
        SetL();
        if (!isServer) {
            CmdSyncXYZ(playerLocation, player);
        } else {
            RpcSyncXYZ(playerLocation, player);
        }
    }

    public void Teleport(Vector3 whereTo) {
        teleportTo = whereTo;
        startTele = true;
    }
    void SetL() {
        playerLocation = player.transform.localPosition;
    }

    [Command]
    void CmdSyncXYZTele(Vector3 i, GameObject player) {
        RpcSyncXYZTele(i, player);
    }

    [ClientRpc]
    void RpcSyncXYZTele(Vector3 i, GameObject playerOut) {
        if (!isLocalPlayer) {
            playerOut.transform.localPosition = i;
        }
    }

    [Command]
    void CmdSyncXYZ(Vector3 i, GameObject player) { 
        RpcSyncXYZ(i, player);
    }

    [ClientRpc]
    void RpcSyncXYZ(Vector3 i, GameObject playerOut) {
        if (!isLocalPlayer) {
            playerOut.transform.localPosition = i;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LookAtMe : NetworkBehaviour {

    public GameObject ChildToLook;
    private GameObject Variables;
    private GameObject manager;
    private PlayerManager playerList;
    private PrepPhase prepCheck;
    private bool addedSelf = false;

    void Awake() {
        Variables = GameObject.FindWithTag("Start");
        manager = Variables.GetComponent<VariablesScript>().variables;
        playerList = manager.GetComponent<PlayerManager>();
        prepCheck = manager.GetComponent<PrepPhase>();
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer) {
            return;
        }
        if (!addedSelf && !prepCheck.inPrep) {
            addedSelf = true;
            foreach(GameObject player in playerList.Players) {
                LookAtMe addSelf = player.GetComponent<LookAtMe>();
                addSelf.ChildToLook.GetComponent<WinningTextFace>().target = this.transform;
            }
        }
    }
}

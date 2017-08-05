using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAssignGet : NetworkBehaviour {

    private GameObject manager;
    private PlayerAssign sm;
    private PlayerManager playerAdd;
    public int currentPlayerNo;
    private PlayerManagerSelf playerArray;
    public GameObject Variables;
    private VariablesScript ManagerGet;

    void Awake() {
        Variables = GameObject.FindWithTag("Start");
    }

    // Use this for initialization
    void Start () {
        ManagerGet = Variables.GetComponent<VariablesScript>();
        manager = ManagerGet.variables;
        sm = manager.GetComponent<PlayerAssign>();
        playerAdd = manager.GetComponent<PlayerManager>();
        playerArray = this.gameObject.GetComponent<PlayerManagerSelf>();

        CmdGetNum();//no authority
        currentPlayerNo = sm.playerNo;
        playerArray.AddSelf(this.gameObject, currentPlayerNo);
        //playerAdd.CmdAddSelf(this.gameObject, currentPlayerNo);
        
	}
    [Command]
    void CmdGetNum() {
        sm.playerNo++;
    }

    // Update is called once per frame
    void Update () {
		
	}
}

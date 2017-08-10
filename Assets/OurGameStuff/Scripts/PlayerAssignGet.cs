using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAssignGet : NetworkBehaviour {

    private GameObject manager;
    private PlayerAssign sm;
    private PlayerManager playerAdd;
    
    private PlayerManagerSelf playerArray;
    public GameObject Variables;
    private VariablesScript ManagerGet;

    [SyncVar]
    public int currentPlayerNo;
    [SyncVar]
    public int kills = 0;
    [SyncVar]
    public int deaths = 0;

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

        CmdGetNum();
       // CmdSetPlayerNum(sm.playerNo);
        
        //playerArray.AddSelf(this.gameObject, currentPlayerNo);
        //playerAdd.CmdAddSelf(this.gameObject, currentPlayerNo);
        
	}
    
    [Command]
    void CmdGetNum() {
        sm.playerNo++;
        currentPlayerNo = sm.playerNo;
    }
    //[Command]
   // void CmdSetPlayerNum(int num) {
    ///    currentPlayerNo = num;
   // }
    [Command]
    public void CmdIncreaseKill() {
        kills++;
    }

    public void takePlayerNumber() {
       // if (isLocalPlayer) {
         //   CmdGetNum();
       //     CmdSetPlayerNum(sm.playerNo);
      //  }
    }

    // Update is called once per frame
    void Update () {
		
	}
}

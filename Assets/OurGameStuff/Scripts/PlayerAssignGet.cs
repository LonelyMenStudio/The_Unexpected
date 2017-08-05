using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssignGet : MonoBehaviour {

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

        sm.doCommand();//no authority
        currentPlayerNo = sm.playerNo;
        playerArray.AddSelf(this.gameObject, currentPlayerNo);
        //playerAdd.CmdAddSelf(this.gameObject, currentPlayerNo);
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

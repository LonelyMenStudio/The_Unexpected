using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssignGet : MonoBehaviour {

    public GameObject manager;
    private PlayerAssign sm;
    private PlayerManager playerAdd;
    public int currentPlayerNo;
    private PlayerManagerSelf playerArray;


    void Awake() {
        manager = GameObject.Find("Manager");
        sm = manager.GetComponent<PlayerAssign>();
        playerAdd = manager.GetComponent<PlayerManager>();
        playerArray = this.gameObject.GetComponent<PlayerManagerSelf>();
    }

	// Use this for initialization
	void Start () {
        sm.CmdGetNum();
        currentPlayerNo = sm.playerNo;
        playerArray.AddSelf(this.gameObject, currentPlayerNo);
        //playerAdd.CmdAddSelf(this.gameObject, currentPlayerNo);
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

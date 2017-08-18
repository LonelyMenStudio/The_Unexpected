using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PrepCheck : NetworkBehaviour {

    //private GameObject prepManager;
    //private PrepPhase prepCheck;
    //public bool prep;
    UnityStandardAssets.Characters.FirstPerson.FirstPersonController con;
    //public GameObject self;
    

    // Use this for initialization
    void Start () {
        if (!isLocalPlayer) {
            Destroy(this);
        }
        //prepManager = GameObject.Find("PrepPhaseManager");
        //prepCheck = prepManager.GetComponent<PrepPhase>();
        con = this.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        
    }
	
	// Update is called once per frame
	void Update () {
        con.enabled = true;//only needs to do it once

        //if (prep) {
        //    con.enabled = false;
        //} else {
        //    con.enabled = true;
        //}
        //prep = prepCheck.inPrep;
    }
}

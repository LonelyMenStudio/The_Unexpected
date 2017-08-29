using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnPrepEnd : MonoBehaviour {

    private bool runOnce = false;
    private GameObject Variables;
    private VariablesScript ManagerGet;
    private GameObject manager;
    private PrepPhase prepPhase;

    // Use this for initialization
    void Start () {
        Variables = GameObject.FindWithTag("Start");
        ManagerGet = Variables.GetComponent<VariablesScript>();
        manager = ManagerGet.variables;
        prepPhase = manager.GetComponent<PrepPhase>();
    }
	
	// Update is called once per frame
	void Update () {
		if(runOnce == true) {
            return;
        }
        if (!prepPhase.inPrep) {
            runOnce = true;
            this.gameObject.SetActive(false);
        }
	}
}

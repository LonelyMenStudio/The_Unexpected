using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLoading : MonoBehaviour {

    private bool runOnce = false;
    private GameObject Variables;
    private VariablesScript ManagerGet;
    private GameObject manager;
    private PrepPhase prepPhase;

    // Use this for initialization
    void Start() {
        Variables = GameObject.FindWithTag("Start");
        ManagerGet = Variables.GetComponent<VariablesScript>();
        manager = ManagerGet.variables;
        prepPhase = manager.GetComponent<PrepPhase>();
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        if (runOnce == true) {
            return;
        }
        this.gameObject.SetActive(true);
        if (prepPhase.checkCounting()) {
            runOnce = true;
            this.gameObject.SetActive(false);
        }
    }
}

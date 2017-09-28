using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtons : MonoBehaviour {

    private GameObject manager;
    private VariablesScript ManagerGet;
    private GameObject Variables;
    private PlayerManager playM;
    private int player;

    private bool hasGot = false;

    void Awake() {
        Variables = GameObject.FindWithTag("Start");
    }

    // Use this for initialization
    void Start() {
        ManagerGet = Variables.GetComponent<VariablesScript>();
        manager = ManagerGet.variables;
        playM = manager.GetComponent<PlayerManager>();

    }

    void getLocal() {
        for (int i = 0; i < playM.Players.Count; i++) {
            if (playM.Players[i].GetComponent<PlayerAssignGet>().isLocal) {
                player = playM.Players[i].GetComponent<PlayerAssignGet>().currentPlayerNo;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (!hasGot && manager.GetComponent<PrepPhase>().halfPrep) {
            hasGot = true;
            getLocal();
        }
    }

    public void resume() {
        for (int i = 0; i < playM.Players.Count; i++) {
            if (playM.Players[i].GetComponent<PlayerAssignGet>().currentPlayerNo == player) {
                playM.Players[i].GetComponent<PauseMenu>().isPaused = false;
            }
        }
    }
    public void controls() {
        //shows contols
    }
    public void quit() {
        if (player == 1) {
            playM.Players[0].GetComponent<PauseMenu>().EndGame();
        } else {

        }
    }
    
}

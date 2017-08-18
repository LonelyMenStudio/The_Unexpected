using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameTimerCommander : NetworkBehaviour {


    private GameObject Variables;
    private VariablesScript ManagerGet;
    private GameObject manager;
    private GameTimer gameTimer;
    public bool isCounter = false;
    private PlayerAssignGet checkPlayer;

    //Remove when ready or just set to true
    private bool useTimer = true;
   


    void Start() {
        Variables = GameObject.FindWithTag("Start");
        ManagerGet = Variables.GetComponent<VariablesScript>();
        manager = ManagerGet.variables;
        gameTimer = manager.GetComponent<GameTimer>();
        checkPlayer = this.gameObject.GetComponent<PlayerAssignGet>();
    }


    void Update() {
        if (!isCounter) {
            return;
        }
            CmdRunCountdown();
    }

    public void TryStart() {
        if (!useTimer) {
            return;
        }
        if(checkPlayer.currentPlayerNo == 1 && isLocalPlayer) {
            isCounter = true;
        }      
    }

    [Command]
    void CmdRunCountdown() {
        gameTimer.Countdown();
    }
}

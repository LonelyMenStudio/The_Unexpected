using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtons : MonoBehaviour {

    private GameObject manager;
    private VariablesScript ManagerGet;
    private GameObject Variables;
    private PlayerManager playM;
    private int player;
    public GameObject Lobby;

    private bool hasGot = false;

    void Awake() {
        Variables = GameObject.FindWithTag("Start");
    }

    // Use this for initialization
    void Start() {
        ManagerGet = Variables.GetComponent<VariablesScript>();
        manager = ManagerGet.variables;
        playM = manager.GetComponent<PlayerManager>();
        Lobby = GameObject.Find("LobbyManager");
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

    }
    public void hideControls() {
    }
    public void quit() {
        if (player == 1) {
            playM.Players[0].GetComponent<PauseMenu>().EndGame();
        } else {
            SceneManager.LoadScene(0);
            Destroy(Lobby);
        }
    }

}

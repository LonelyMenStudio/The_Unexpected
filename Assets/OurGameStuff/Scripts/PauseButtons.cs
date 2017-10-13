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
    private GameObject controlsScreen;
    private bool canButton = true;

    private PrepPhase getPrep;

    private GameObject resumeButton;
    private GameObject controlsButton;
    private GameObject audioButton;
    private GameObject quitButton;
    private GameObject audioOptions;
    private GameObject controlsResume;


    private bool hasGot = false;

    void Awake() {
        Variables = GameObject.FindWithTag("Start");
    }

    // Use this for initialization
    void Start() {
        ManagerGet = Variables.GetComponent<VariablesScript>();
        manager = ManagerGet.variables;
        playM = manager.GetComponent<PlayerManager>();
        getPrep = manager.GetComponent<PrepPhase>();
        Lobby = GameObject.Find("LobbyManager");
        controlsScreen = getPrep.inGameControls;

        resumeButton = getPrep.resumeButton;
        controlsButton = getPrep.controlsButton;
        audioButton = getPrep.audioButton;
        audioOptions = getPrep.audioOptions;
        quitButton = getPrep.quitButton;
        controlsResume = getPrep.controlsMenuResume;
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
        if(canButton == false && Input.GetKeyDown(KeyCode.Space)) {
            hideControls();
        }
    }

    public void resume() {
        if (!canButton) {
            return;
        }
        for (int i = 0; i < playM.Players.Count; i++) {
            if (playM.Players[i].GetComponent<PlayerAssignGet>().currentPlayerNo == player) {
                playM.Players[i].GetComponent<PauseMenu>().isPaused = false;
            }
        }

    }
    public void controls() { // Redo
        if (!canButton) {
            return;
        }
        controlsScreen.SetActive(true);
        setButtonVisibility(false);
        controlsResume.SetActive(true);
        canButton = false;
    }

    public void audioOptionsButton() {
        setButtonVisibility(false);
        audioOptions.SetActive(true);
    }

    public void returnToMenu() {
        audioOptions.SetActive(false);
        setButtonVisibility(true);
    }

    private void setButtonVisibility(bool state) {
        resumeButton.SetActive(state);
        controlsButton.SetActive(state);
        audioButton.SetActive(state);
        quitButton.SetActive(state);
    }

    public void hideControls() {
        setButtonVisibility(true);
        controlsScreen.SetActive(false);
        controlsResume.SetActive(false);
        canButton = true;
    }
    public void quit() {
        if (!canButton) {
            return;
        }
        if (player == 1) {
            playM.Players[0].GetComponent<PauseMenu>().EndGame();
        } else {
            SceneManager.LoadScene(0);
            Destroy(Lobby);
        }
    }

}

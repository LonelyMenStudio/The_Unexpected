using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PauseMenu : NetworkBehaviour {

    private GameObject manager;
    private VariablesScript ManagerGet;
    private GameObject Variables;
    public bool isPaused;
    private PrepPhase getPrep;
    private GameObject pauseMenu;
    UnityStandardAssets.Characters.FirstPerson.FirstPersonController con;
    private GameObject controls;
    private PauseButtons leaveMenu;



    void Awake() {
        Variables = GameObject.FindWithTag("Start");
    }


    // Use this for initialization
    void Start() {
        ManagerGet = Variables.GetComponent<VariablesScript>();
        manager = ManagerGet.variables;
        getPrep = manager.GetComponent<PrepPhase>();
        con = this.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        pauseMenu = getPrep.pauseMenu;
        controls = getPrep.inGameControls;
        leaveMenu = getPrep.optionMenu.GetComponent<PauseButtons>();


    }

    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && manager.GetComponent<PrepPhase>().halfPrep) { // For resume button to work :( hopefully can improve
            if (isPaused && leaveMenu.inSeperatMenu) {
                leaveMenu.returnToMenu();
                leaveMenu.hideControls();
            }
            isPaused = !isPaused;

        }
        if (isPaused) {
            pauseMenu.SetActive(true);
            this.gameObject.GetComponent<PrepCheck>().stop = true;
            con.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        } else {
            controls.SetActive(false);
            pauseMenu.SetActive(false);
            this.gameObject.GetComponent<PrepCheck>().stop = false;
        }
    }



    public void EndGame() {
        CmdEndGame();
    }
    [Command]
    private void CmdEndGame() {
        manager.gameObject.GetComponent<GameTimer>().gameTime = 0;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PauseMenu : NetworkBehaviour {

    private GameObject manager;
    private VariablesScript ManagerGet;
    private GameObject Variables;
    public bool isPaused;
    private GameObject pauseMenu;
    UnityStandardAssets.Characters.FirstPerson.FirstPersonController con;


    void Awake() {
        Variables = GameObject.FindWithTag("Start");
    }


    // Use this for initialization
    void Start() {
        ManagerGet = Variables.GetComponent<VariablesScript>();
        manager = ManagerGet.variables;
        pauseMenu = manager.GetComponent<PrepPhase>().pauseMenu;
        con = this.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }

    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = !isPaused;
        }
        if (isPaused) {
            pauseMenu.SetActive(true);
            this.gameObject.GetComponent<PrepCheck>().stop = true;
            con.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        } else {
            pauseMenu.SetActive(false);
            this.gameObject.GetComponent<PrepCheck>().stop = false;
        }
    }

    public void EndGame() {
        CmdEndGame();
    }
    [Command]
    private void CmdEndGame() {
        this.gameObject.GetComponent<GameTimer>().gameTime = 0;
    }

}

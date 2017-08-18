using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : NetworkBehaviour {

    private const float GAME_TIME_LENGTH = 120;
    public GameObject timerObject;
    private Text timer;
    private string timeDisplay = "";
    public bool gameTimeOver = false;
    private PrepPhase scoreboard;
    private PlayerManager playerManager;
    

    [SyncVar]
    public float gameTime = GAME_TIME_LENGTH;

    int timerMinutes;
    int timerSeconds;

    void getTime() {
        timerMinutes = (int)(gameTime / 60);
        timerSeconds = (int)(gameTime - (timerMinutes * 60));
        timeDisplay = timerMinutes + ":" + timerSeconds;
    }

    public void Countdown() {
        if (!isServer || gameTimeOver) {
            return;
        }
        gameTime -= Time.deltaTime;
        if (gameTime <= 0) {
            gameTime = 0;
            gameTimeOver = true;
        }
    }


    // Use this for initialization
    void Start() {
        timer = timerObject.GetComponent<Text>();
        scoreboard = this.gameObject.GetComponent<PrepPhase>();
        playerManager = this.gameObject.GetComponent<PlayerManager>();
    }


    // Update is called once per frame
    void Update() {
        getTime();
        //timer.text = timeDisplay; // need to uncomment
        if(gameTimeOver == true) {
            UnlockMouse();
            //game over camera maybe
            ShowScoreboard();
            LoadMainMenu();
        }
    }

    void ShowScoreboard() {
        scoreboard.PlayerScores.gameObject.SetActive(true);
        //stop it constantlybeing hidden by other script
        //need to do something in PlayerAssignGet for this but cant right now
    }

    void UnlockMouse() {
        for(int i = 0; i < playerManager.Players.Count; i++) {
            UnityStandardAssets.Characters.FirstPerson.FirstPersonController Controller = playerManager.Players[i].GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
            Controller.enabled = false;
        }
        //change prep check
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator LoadMainMenu() {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(0);
    }

}

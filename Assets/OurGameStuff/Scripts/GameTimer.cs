using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameTimer : NetworkBehaviour {

    private const float GAME_TIME_LENGTH = 300;
    public GameObject timerObject;
    private Text timer;
    private string timeDisplay = "";
    private PrepPhase scoreboard;
    private PlayerManager playerManager;
    int timerMinutes;
    int timerSeconds;
    public bool halfTime = false;
    public bool outOfPrep = false;
    public bool timerStarted = false;

    [SyncVar]
    public float gameTime = GAME_TIME_LENGTH;
    [SyncVar]
    public bool gameTimeOver = false;


    void getTime() {
        timerMinutes = (int)(gameTime / 60);
        timerSeconds = (int)(gameTime - (timerMinutes * 60));
        string min, sec;
        if (timerMinutes < 10) {
            min = "0" + timerMinutes;
        } else {
            min = "" + timerMinutes;
        }
        if (timerSeconds < 10) {
            sec = "0" + timerSeconds;
        } else {
            sec = "" + timerSeconds;
        }
        timeDisplay = min + ":" + sec;
        if (gameTime <= (GAME_TIME_LENGTH / 2)) {
            halfTime = true;
        } else {
            halfTime = false;
        }
    }

    public void Countdown() {
        if (!isServer || gameTimeOver) {
            return;
        }
        gameTime -= Time.deltaTime;
        if (gameTime <= 0) {
            gameTime = 0;
            gameTimeOver = true;
            StartCoroutine(LoadMainMenu());
        }
    }


    // Use this for initialization
    void Start() {
        scoreboard = this.gameObject.GetComponent<PrepPhase>();
        playerManager = this.gameObject.GetComponent<PlayerManager>();
        timer = timerObject.GetComponent<Text>();
    }


    // Update is called once per frame
    void Update() {
        outOfPrep = !scoreboard.inPrep;
        getTime();
        timer.text = timeDisplay;
        if (gameTimeOver == true) {
            ShowScoreboard();
            UnlockMouse();
            //game over camera maybe
        }
        if (gameTime < 60) {
            timerObject.SetActive(true);
        }
        if (gameTime < GAME_TIME_LENGTH) {
            timerStarted = true;
        }
    }

    void ShowScoreboard() {
        scoreboard.playerStats.gameObject.SetActive(true);
    }

    void UnlockMouse() {
        for (int i = 0; i < playerManager.Players.Count; i++) {
            UnityStandardAssets.Characters.FirstPerson.FirstPersonController Controller = playerManager.Players[i].GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
            Controller.gameObject.GetComponent<PrepCheck>().stop = true;
            Controller.enabled = false;
        }
        //change prep check
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator LoadMainMenu() {
        yield return new WaitForSeconds(10);
        FindObjectOfType<NetworkLobbyManager>().ServerReturnToLobby();
    }

}

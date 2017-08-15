using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameTimer : NetworkBehaviour {

    private const float GAME_TIME_LENGTH = 120;
    public GameObject timerObject;
    private Text timer;
    private string timeDisplay = "";
    private bool gameTimeOver = false;

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
    }


    // Update is called once per frame
    void Update() {
        getTime();
        timer.text = timeDisplay;
        if(gameTimeOver == true) {
            //lock player out
            //Show scores for amount of time
            //Load Main Menu
        }
    }
}

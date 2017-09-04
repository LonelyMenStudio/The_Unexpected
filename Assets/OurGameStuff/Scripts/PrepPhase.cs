using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PrepPhase : MonoBehaviour {

    private const float TOTAL_PREP_TIME = 20;
    public GameObject[] spawn;
    private float timeRemaining = TOTAL_PREP_TIME;
    private string tr = "Entering Battle in: ";
    private string timeR = "";
    public GameObject timer;
    private Text text;
    public bool inPrep = true;
    public bool teleport = false;
    public bool playwep = false;
    public GameObject ammoObject;
    public GameObject healthObject;
    public GameObject PlayerHUD;
    public GameObject HitMark;
    public GameObject PlayerScores;
    public GameObject HitScreen;
    public GameObject HUGgun1;
    public GameObject HUGgun2;
    public GameObject HUGgun3;
    public GameObject HUGgun4;
    public GameObject HUGgun5;
    public GameObject HUGgun6;
    public List<GameObject> Players = new List<GameObject>();
    private PlayerManager assignTime;
    private bool canAssign = true;
    public GameObject ErrorText;
    private GameTimerCommander timerStarter;


    // Use this for initialization
    void Start() {
        text = timer.GetComponent<Text>();
        assignTime = this.gameObject.GetComponent<PlayerManager>();
        ErrorText = GameObject.FindWithTag("ErrorText");
        ErrorText.SetActive(false);     
    }

    // Update is called once per frame
    void Update() {
        for (int i = 1; i < Players.Count; i++) {
            Players[i].name = "Player " + i;
        }

        if (inPrep) {
            timeRemaining -= Time.deltaTime;
            timeR = timeRemaining.ToString("F1");
            text.text = tr + timeR;

        }
        if(timeRemaining <= 10 && canAssign) {
            assignTime.assignPlayerNumbers();
            canAssign = false;
        }
        if (timeRemaining <= 0) {
            teleport = true;
            playwep = true;
            timeRemaining = 2;
            inPrep = false;
            timer.SetActive(false);
            for(int i = 0; i < Players.Count; i++) {
                timerStarter = Players[i].gameObject.GetComponent<GameTimerCommander>();
                timerStarter.TryStart();
            }
            ErrorText.SetActive(true);
            StartCoroutine(ShowError());

        }
        if (teleport == true) {
            for (int i = 0; i < Players.Count; i++) {
                int indexspawn = Random.Range(0, spawn.Length);
                Players[i].transform.position = spawn[indexspawn].transform.position;
                teleport = false;
            }
        }

    }
    public bool checkCounting() {
        if(timeRemaining < TOTAL_PREP_TIME) {
            return true;
        } else {
            return false;
        }
    }
    IEnumerator ShowError() {
        yield return new WaitForSeconds(6);
        ErrorText.SetActive(false);
    }
}

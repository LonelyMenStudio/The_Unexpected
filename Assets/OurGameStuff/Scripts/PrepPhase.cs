using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PrepPhase : MonoBehaviour {

    public GameObject[] spawn;
    private float timeRemaining = 20;
    private string tr = "Entering Battle in: ";
    private string timeR = "";
    public GameObject timer;
    private Text text;
    public bool inPrep = true;
    public bool teleport = false;
    public bool playwep = false;
    public GameObject ammoObject;
    //public GameObject[] players;
    public GameObject healthObject;
    public GameObject PlayerHUD;
    public GameObject HitMark;
    public GameObject PlayerScores;
    public GameObject HitScreen;
    public List<GameObject> Players = new List<GameObject>();
    // public int playerIDs = 0;
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
        //players = GameObject.FindWithTag("Player");
        
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
            /*      if (players == null)
             players = GameObject.FindGameObjectsWithTag("Player");
             foreach (GameObject player in players) {

                 int indexspawn = Random.Range(0, spawn.Length);
                 player.transform.position = spawn[indexspawn].transform.position;
                 teleport = false;
             }*/
        }

    }
    IEnumerator ShowError() {
        yield return new WaitForSeconds(4);
        ErrorText.SetActive(false);
    }




}

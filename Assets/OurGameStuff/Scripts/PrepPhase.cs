using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PrepPhase : MonoBehaviour {

    public GameObject[] spawn;
    public float timeRemaining = 15;
    private string tr = "Time Remaining: ";
    private string timeR = "";
    public GameObject timer;
    private Text text;
    public bool inPrep = true;
    public bool teleport = false;
    public bool playwep = false;
    public GameObject ammoObject;
    //public GameObject[] players;
    public List<GameObject> Players = new List<GameObject>();
    // public int playerIDs = 0;


    // Use this for initialization
    void Start() {
        text = timer.GetComponent<Text>();
        //players = GameObject.FindWithTag("Player");
        for(int i = 0; i < Players.Count; i++) {
            Players[i].name = "Player " + i; 
        }
    }

    // Update is called once per frame
    void Update() {


        if (inPrep) {
            timeRemaining -= Time.deltaTime;
            timeR = timeRemaining.ToString("F1");
            text.text = tr + timeR;

        }
        if (timeRemaining <= 0) {
            teleport = true;
            playwep = true;
            timeRemaining = 2;
            inPrep = false;
            timer.SetActive(false);
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



}

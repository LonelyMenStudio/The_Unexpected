using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour {

    private const int NUMBER_OF_PLAYERS = 4;

    public GameObject[] playerNamesG = new GameObject[NUMBER_OF_PLAYERS];
    public Text[] playerNames;
    public GameObject[] playerKillsG = new GameObject[NUMBER_OF_PLAYERS];
    public Text[] playerKills;
    public GameObject[] playerDeathsG = new GameObject[NUMBER_OF_PLAYERS];
    public Text[] playerDeaths;

    // Use this for initialization
    void Start() {
        playerNames = new Text[NUMBER_OF_PLAYERS];
        playerKills = new Text[NUMBER_OF_PLAYERS];
        playerDeaths = new Text[NUMBER_OF_PLAYERS];
        for (int i = 0; i < NUMBER_OF_PLAYERS; i++) {
            playerNames[i] = playerNamesG[i].GetComponent<Text>();
        }
        for (int i = 0; i < NUMBER_OF_PLAYERS; i++) {
            playerKills[i] = playerKillsG[i].GetComponent<Text>();
        }
        for (int i = 0; i < NUMBER_OF_PLAYERS; i++) {
            playerDeaths[i] = playerDeathsG[i].GetComponent<Text>();
        }
    }

}

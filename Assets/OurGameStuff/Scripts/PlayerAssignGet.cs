using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerAssignGet : NetworkBehaviour {

    private GameObject manager;
    private PlayerAssign sm;
    private PlayerManager playerAdd;
    public GameObject PlayerScore;
    private PlayerManagerSelf playerArray;
    public GameObject Variables;
    private PrepPhase PrepPhase;
    private VariablesScript ManagerGet;
    private Transform PlayerKD;
    private Text DisplayKD;
    private GameTimer checkGameState;
    public GameObject targetMe;


    [SyncVar]
    public int currentPlayerNo;
    [SyncVar]
    public int kills = 0;
    [SyncVar]
    public int deaths = 0;
    [SyncVar]
    public string playerName;
    [SyncVar]
    public bool isWinning = false;

    void Awake() {
        Variables = GameObject.FindWithTag("Start");
    }

    // Use this for initialization
    void Start() {

        ManagerGet = Variables.GetComponent<VariablesScript>();
        manager = ManagerGet.variables;
        sm = manager.GetComponent<PlayerAssign>();
        playerAdd = manager.GetComponent<PlayerManager>();
        playerArray = this.gameObject.GetComponent<PlayerManagerSelf>();
        checkGameState = manager.GetComponent<GameTimer>();
        PrepPhase = manager.GetComponent<PrepPhase>();
        PlayerScore = PrepPhase.PlayerScores;

        PlayerScore.SetActive(false);
        if (isLocalPlayer) {
            CmdGetNum();
        }
        SetupPlayerKD();
    }


    // Update is called once per frame
    void Update() {
        if (checkGameState.halfTime && isWinning && checkGameState.outOfPrep) {
            targetMe.SetActive(true);
        } else {
            targetMe.SetActive(false);
        }
        SetupScoreboard();
        if (!isLocalPlayer || checkGameState.gameTimeOver) { //uncomment when everything added into scene currently would just error
            return;
        }
        if (Input.GetKey(KeyCode.Tab)) {
            PlayerScore.SetActive(true);
        } else {
            PlayerScore.SetActive(false);
        }
        if (isLocalPlayer) {
            targetMe.SetActive(false);
        }
    }

    private void SetupPlayerKD() {
        if (currentPlayerNo == 1) {
            PlayerKD = PlayerScore.transform.Find("Player1");
        } else if (currentPlayerNo == 2) {
            PlayerKD = PlayerScore.transform.Find("Player2");
        } else if (currentPlayerNo == 3) {
            PlayerKD = PlayerScore.transform.Find("Player3");
        } else if (currentPlayerNo == 4) {
            PlayerKD = PlayerScore.transform.Find("Player4");
        } else {
            Debug.Log("No Player found");
        }
    }

    private void SetupScoreboard() {
        DisplayKD = PlayerKD.GetComponent<Text>();
        DisplayKD.text = (playerName + "       " + kills + "        " + deaths);
        /*
        if (currentPlayerNo == 1) {
            PlayerKD = PlayerScore.transform.Find("Player1");
            DisplayKD = PlayerKD.GetComponent<Text>();
            DisplayKD.text = (playerName + "       " + kills + "        " + deaths);
        } else if (currentPlayerNo == 2) {
            PlayerKD = PlayerScore.transform.Find("Player2");
            DisplayKD = PlayerKD.GetComponent<Text>();
            DisplayKD.text = (playerName + "       " + kills + "        " + deaths);
        } else if (currentPlayerNo == 3) {
            PlayerKD = PlayerScore.transform.Find("Player3");
            DisplayKD = PlayerKD.GetComponent<Text>();
            DisplayKD.text = (playerName + "       " + kills + "        " + deaths);
        } else if (currentPlayerNo == 4) {
            PlayerKD = PlayerScore.transform.Find("Player4");
            DisplayKD = PlayerKD.GetComponent<Text>();
            DisplayKD.text = (playerName + "       " + kills + "        " + deaths);
        } else {
            Debug.Log("No Player found");
        }
        */
    }

    [Command]
    void CmdGetNum() {
        sm.playerNo++;
        currentPlayerNo = sm.playerNo;
    }

    [Command]
    public void CmdIncreaseKill() {
        kills++;
    }

    [Command]
    public void CmdLoseKill() {
        kills--;
    }

    public void SetWinning(bool i) {
        if (isLocalPlayer) {
            CmdSetWinning(i);
        }
    }

    [Command]
    void CmdSetWinning(bool i) {
        isWinning = i;
    }
}

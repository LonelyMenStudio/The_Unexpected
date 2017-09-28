using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerAssignGet : NetworkBehaviour {

    private GameObject manager;
    private PlayerAssign sm;
    private PlayerManager playerAdd;
    //public GameObject PlayerScore;
    private PlayerManagerSelf playerArray;
    public GameObject Variables;
    private PrepPhase PrepPhase;
    private VariablesScript ManagerGet;
    private Transform PlayerKD;
    private Text DisplayKD;
    private GameTimer checkGameState;
    public GameObject targetMe;
    private int killsOld = 0;
    private GameObject showKill;
    private GameObject timerDisplay;
    private ScoreScreen stats;
    private GameObject statsDisplay;


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
        stats = PrepPhase.playerStats.GetComponent<ScoreScreen>();
        timerDisplay = PrepPhase.displayTimer;
        //PlayerScore = PrepPhase.PlayerScores;
        showKill = PrepPhase.killGetMessage;
        //PlayerScore.SetActive(false);
        statsDisplay = PrepPhase.playerStats;
        statsDisplay.SetActive(false);

        if (isLocalPlayer) {
            CmdGetNum();
            checkKills();
        }
        // CmdSetPlayerNum(sm.playerNo);

        //playerArray.AddSelf(this.gameObject, currentPlayerNo);
        //playerAdd.CmdAddSelf(this.gameObject, currentPlayerNo);
        //SetupPlayerKD();
    }


    // Update is called once per frame
    void Update() {
        //if (isLocalPlayer) {
        //  if (Input.GetKeyDown(KeyCode.M)) {
        //    Debug.Log(currentPlayerNo);
        //}
        //}

        if (checkGameState.halfTime && isWinning && checkGameState.outOfPrep) {
            targetMe.SetActive(true);
        } else {
            targetMe.SetActive(false);
        }
        SetupScoreboard();
        if (!isLocalPlayer || checkGameState.gameTimeOver) { //uncomment when everything added into scene currently would just error
            return;
        }
        if (checkGameState.gameTime < 60) {
            timerDisplay.SetActive(true);
        } else {
            if (Input.GetKey(KeyCode.Tab)) {
                timerDisplay.SetActive(true);
            } else {
                timerDisplay.SetActive(false);
            }
        }
        if (Input.GetKey(KeyCode.Tab)) {
            //PlayerScore.SetActive(true);
            statsDisplay.SetActive(true);
            //timerDisplay.SetActive(true);
        } else {
            //PlayerScore.SetActive(false);
            statsDisplay.SetActive(false);
            //timerDisplay.SetActive(false);
        }

        if (isLocalPlayer) {
            targetMe.SetActive(false);
            checkKills();
        }
        /*
        if (Input.GetKeyDown(KeyCode.K)) {
            CmdTestKill();
        }
        */
    }

    [Command]
    private void CmdTestKill() {
        kills++;
    }

    void checkKills() {
        if (killsOld < kills) {
            StartCoroutine(GotKill());
        }
        killsOld = kills;
    }

    IEnumerator GotKill() {
        showKill.SetActive(true);
        yield return new WaitForSeconds(2f);
        showKill.SetActive(false);
    }

    /*
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
    */

    private void SetupScoreboard() {
        //DisplayKD = PlayerKD.GetComponent<Text>();
        //DisplayKD.text = (playerName + "       " + kills + "        " + deaths);

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
        if (playerName == "") {
            playerName = "Player";
        }

        if (currentPlayerNo == 1) {
            stats.playerNames[0].text = playerName;
            stats.playerKills[0].text = "" + kills;
            stats.playerDeaths[0].text = "" + deaths;
        } else if (currentPlayerNo == 2) {
            stats.playerNames[1].text = playerName;
            stats.playerKills[1].text = "" + kills;
            stats.playerDeaths[1].text = "" + deaths;
        } else if (currentPlayerNo == 3) {
            stats.playerNames[2].text = playerName;
            stats.playerKills[2].text = "" + kills;
            stats.playerDeaths[2].text = "" + deaths;
        } else if (currentPlayerNo == 4) {
            stats.playerNames[3].text = playerName;
            stats.playerKills[3].text = "" + kills;
            stats.playerDeaths[3].text = "" + deaths;
        }

        //stats.playerNames[currentPlayerNo].text = playerName;
        //stats.playerKills[currentPlayerNo].text = "" + kills;
        //stats.playerDeaths[currentPlayerNo].text = "" + deaths;
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

    public void takePlayerNumber() {
        // if (isLocalPlayer) {
        //   CmdGetNum();
        //     CmdSetPlayerNum(sm.playerNo);
        //  }
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

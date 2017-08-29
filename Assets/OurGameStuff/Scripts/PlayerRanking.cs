using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRanking : MonoBehaviour {

    private PlayerManager playerList;

    // Use this for initialization
    void Start() {
        playerList = this.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update() {
        //if !prep
        if (this.GetComponent<GameTimer>().outOfPrep) {
            playerRanking();
        }
    }

    void playerRanking() {
        int pn = playerList.Players.Count;
        //int[] playerNumber = new int[pn];
        int[] playerKills = new int[pn];
        for (int i = 0; i < pn; i++) {
            //playerNumber[i] = playerList.Players[i].GetComponent<PlayerAssignGet>().currentPlayerNo;
            playerKills[i] = playerList.Players[i].GetComponent<PlayerAssignGet>().kills;
        }
        int highestKills = playerKills[0];
        foreach (int kills in playerKills) {
            if (kills > highestKills) {
                highestKills = kills;
            }
        }
        for (int i = 0; i < pn; i++) {
            PlayerAssignGet s = playerList.Players[i].GetComponent<PlayerAssignGet>();
            if (playerKills[i] == highestKills) {
                s.SetWinning(true);
            } else {
                s.SetWinning(false);
            }
        }
    }
}

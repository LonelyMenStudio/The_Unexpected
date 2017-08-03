﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour {

   // [SyncVar]
    public GameObject[] players = new GameObject[5];//in future could set to dynamic length array//pssibly not work as mght nee to be a command// need to be a ync list

    //[SyncVar]
    public List<GameObject> droppedWeapons = new List<GameObject>();
    //public SyncList<GameObject>

    public int numberOfPlayers = 5;//set up for 4 players but for prototype should probs only have 2// 5 because i forgot things and reasons
    //array of weapons empty
    public GameObject[] weaponPortLocations;
    public GameObject managerObject;
    private PlayerAssign playerAssign;
   // public Transform weapon1, weapon2, weapon3, weapon4;// will need a dynamic length array
    

    [SyncVar]
    public bool player1Dead;
    [SyncVar]
    public bool player2Dead; 
    [SyncVar]
    public bool player3Dead;
    [SyncVar]
    public bool player4Dead;


    
    


    /*
    [Command]
    public void CmdAddWeapon(GameObject weapon) {
        droppedWeapons.Add(weapon);
    }
    [Command]
    public void CmdRemoveWeapon(GameObject weapon) {
        droppedWeapons.Remove(weapon);
    }
    */

	// Use this for initialization
	void Start () {
        playerAssign = managerObject.GetComponent<PlayerAssign>();
        weaponPortLocations = new GameObject[8];
        for (int i = 0; i < 8; i++) {
            weaponPortLocations[i] = playerAssign.weaponRespawnPoints[i];
        }
    }


    /*
    [Command]
    void CmdDamageDealer(GameObject hit,  int damage) {
        hit.SendMessage("TakeDamage", currentWeaponDamage);
    }

        server is sending the take damage signal

         public void TakeDamage(int amount) {
        if (!isServer) {
            return;
        }
        Healthz -= amount;
        if(Healthz <= 0) {
            Healthz = 0;
        }

       
    }

    weapons will be a bit iffy and it will need a variable list

    */

    // Update is called once per frame
    void Update () {
        if (player1Dead) {
            for(int i = 1; i < numberOfPlayers; i++) {
                CmdSendMessageEach(players[i], 1);
                checkWeapons(1);
            }
            Cmd1Died();
        }
        if (player2Dead) {
            for (int i = 1; i < numberOfPlayers; i++) {
                CmdSendMessageEach(players[i], 2);
                checkWeapons(2);
            }
            Cmd2Died();
        }
        if (player3Dead) {
            for (int i = 1; i < numberOfPlayers; i++) {
                CmdSendMessageEach(players[i], 3);
                checkWeapons(3);
            }
            Cmd3Died();
        }
        if (player4Dead) {
            for (int i = 1; i < numberOfPlayers; i++) {
                CmdSendMessageEach(players[i], 4);
                checkWeapons(4);
            }
            Cmd4Died();
        }
    }
    [Command]
    void CmdSendMessageEach(GameObject player, int val) {//possibly could rework
        player.SendMessage("CheckWeaponNumber", val);
    }
    [Command]
    public void CmdPlayerDied(int playerNo) {
        if(playerNo == 1) {
            player1Dead = true;
        }
        if (playerNo == 2) {
            player2Dead = true;
        }
        if (playerNo == 3) {
            player3Dead = true;
        }
        if (playerNo == 4) {
            player4Dead = true;
        }
    }
    [Command]
    void Cmd1Died() {
        player1Dead = false;
    }
    [Command]
    void Cmd2Died() {
        player2Dead = false;
    }
    [Command]
    void Cmd3Died() {
        player3Dead = false;
    }
    [Command]
    void Cmd4Died() {
        player4Dead = false;
    }
    private void checkWeapons(int playerThatDied) {
        foreach(GameObject weapon in droppedWeapons) {
            weaponSettings weaponPlayerCheck = weapon.GetComponent<weaponSettings>();
            if(playerThatDied == weaponPlayerCheck.playerNo) {
                weapon.transform.position = weaponPortLocations[Random.Range(0, 7)].transform.position;
                return;
            }
        }
    }
}

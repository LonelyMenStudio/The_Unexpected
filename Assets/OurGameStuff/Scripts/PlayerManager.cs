using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour {

    //public GameObject[] players = new GameObject[5];//useless - to remove
    public List<GameObject> Players = new List<GameObject>();
    public List<GameObject> droppedWeapons = new List<GameObject>();
    //public SyncList<GameObject>

    //public int numberOfPlayers = 5;//set up for 4 players but for prototype should probs only have 2// 5 because i forgot things and reasons
    //array of weapons empty
    public GameObject[] weaponPortLocations;
    private PlayerAssign playerAssign;
    // public Transform weapon1, weapon2, weapon3, weapon4;// will need a dynamic length array
    private GameObject[] playerSpawner;
    private PrepPhase getSpawns;

/*
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
        playerAssign = this.gameObject.GetComponent<PlayerAssign>();
        getSpawns = this.gameObject.GetComponent<PrepPhase>();
        weaponPortLocations = new GameObject[playerAssign.weaponRespawnPoints.Length];
        playerSpawner = new GameObject[getSpawns.spawn.Length];
        for (int i = 0; i < playerAssign.weaponRespawnPoints.Length; i++) {
            weaponPortLocations[i] = playerAssign.weaponRespawnPoints[i];
        }
        for (int i = 0; i < getSpawns.spawn.Length; i++) {
            playerSpawner[i] = getSpawns.spawn[i];
        }
    }
    public bool finishedWaiting = true;
    public void assignPlayerNumbers() {
        for(int i = 0; i < Players.Count; i++) {
            PlayerAssignGet sendPlayerNumber = Players[i].GetComponent<PlayerAssignGet>();
            sendPlayerNumber.takePlayerNumber();
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
        scanForMissing();
        /*
        if (player1Dead) {
            for(int i = 0; i < Players.Count; i++) {
                SendMessageEach(Players[i], 1);
                checkWeapons(1);
            }
            //Cmd1Died();
        }
        if (player2Dead) {
            for (int i = 0; i < Players.Count; i++) {
                SendMessageEach(Players[i], 2);
                checkWeapons(2);
            }
            //Cmd2Died();
        }
        if (player3Dead) {
            for (int i = 0; i < Players.Count; i++) {
                SendMessageEach(Players[i], 3);
                checkWeapons(3);
            }
            //Cmd3Died();
        }
        if (player4Dead) {
            for (int i = 0; i < Players.Count; i++) {
                SendMessageEach(Players[i], 4);
                checkWeapons(4);
            }
            //Cmd4Died();
        }
        */
    }
    //[ClientRpc]
    //void telePlayer(int i) {
    //    Players[i].transform.position = playerSpawner[Random.Range(0, playerSpawner.Length)].transform.position;
    //}

    public void deathMessenger(int playerDead) {
        /* teleport trying
        for (int i = 0; i < Players.Count; i++) {
            PlayerAssignGet respawner = Players[i].GetComponent<PlayerAssignGet>();
            if (playerDead == respawner.currentPlayerNo) {
                //telePlayer(i);
                Players[i].transform.position = playerSpawner[Random.Range(0, playerSpawner.Length)].transform.position;
            }
        }
        */
        if (playerDead == 1) {
            for (int i = 0; i < Players.Count; i++) {
                SendMessageEach(Players[i], 1);
                checkWeapons(1);
            }
            //Cmd1Died();
        }
        if (playerDead == 2) {
            for (int i = 0; i < Players.Count; i++) {
                SendMessageEach(Players[i], 2);
                checkWeapons(2);
            }
            //Cmd2Died();
        }
        if (playerDead == 3) {
            for (int i = 0; i < Players.Count; i++) {
                SendMessageEach(Players[i], 3);
                checkWeapons(3);
            }
            //Cmd3Died();
        }
        if (playerDead == 4) {
            for (int i = 0; i < Players.Count; i++) {
                SendMessageEach(Players[i], 4);
                checkWeapons(4);
            }
            //Cmd4Died();
        }
        //need to redo wmaybe because of wonky player numbers
    }
    

    void SendMessageEach(GameObject player, int val) {//possibly could rework
        player.SendMessage("CheckWeaponNumber", val);
    }

    /*
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
    */
    private void checkWeapons(int playerThatDied) {
        foreach(GameObject weapon in droppedWeapons) {
            weaponSettings weaponPlayerCheck = weapon.GetComponent<weaponSettings>();
            if(playerThatDied == weaponPlayerCheck.playerNo) {
                weapon.transform.position = weaponPortLocations[Random.Range(0, 7)].transform.position;// not a command and therfore needs update
                return;
            }
        }
    }

    private void scanForMissing() {
       droppedWeapons.RemoveAll(item => item == null);
    }
}

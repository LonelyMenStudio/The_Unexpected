using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour {

    public List<GameObject> Players = new List<GameObject>();
    public List<GameObject> droppedWeapons = new List<GameObject>();
    public GameObject[] weaponPortLocations;
    private PlayerAssign playerAssign;
    private GameObject[] playerSpawner;
    private PrepPhase getSpawns;

    // Use this for initialization
    void Start() {

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
        for (int i = 0; i < Players.Count; i++) {
            PlayerAssignGet sendPlayerNumber = Players[i].GetComponent<PlayerAssignGet>();
            sendPlayerNumber.takePlayerNumber();
        }
    }

    // Update is called once per frame
    void Update() {
        scanForMissing();
    }

    public void deathMessenger(int playerDead) {
        if (playerDead == 1) {
            for (int i = 0; i < Players.Count; i++) {
                checkWeapons(1);
            }
        }
        if (playerDead == 2) {
            for (int i = 0; i < Players.Count; i++) {
                checkWeapons(2);
            }
        }
        if (playerDead == 3) {
            for (int i = 0; i < Players.Count; i++) {
                checkWeapons(3);
            }
        }
        if (playerDead == 4) {
            for (int i = 0; i < Players.Count; i++) {
                checkWeapons(4);
            }
        }
    }

    private void checkWeapons(int playerThatDied) {
        foreach (GameObject weapon in droppedWeapons) {
            weaponSettings weaponPlayerCheck = weapon.GetComponent<weaponSettings>();
            if (playerThatDied == weaponPlayerCheck.playerNo) {
                weapon.transform.position = weaponPortLocations[Random.Range(0, 7)].transform.position;// not a command and therfore needs update
                return;
            }
        }
    }

    private void scanForMissing() {
        droppedWeapons.RemoveAll(item => item == null);
    }
}

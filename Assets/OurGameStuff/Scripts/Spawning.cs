using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Spawning : NetworkBehaviour {
    public Transform[] Pistol;
    public GameObject Armour;
    public GameObject Medic;
    public Transform[] upB;
    public Transform[] Outside;
    public Transform[] upA;
    public Transform[] BackofB;
    public Transform[] BackofA;
    //public Transform[] PlayerSpawn;
    public GameObject Player;
    private bool keepSpawn = true;
    private GameObject ob;
    private Manager vals;
    public GameObject prep;
    private PrepPhase pChck;


    // Use this for initialization
    void Start() {
        ob = this.gameObject;
        vals = ob.GetComponent<Manager>();
        //int points = Random.Range(0, PlayerSpawn.Length);
        //Instantiate(Player, PlayerSpawn[points].transform.position, Quaternion.identity);
        //Player.SetActive(true);
        pChck = prep.GetComponent<PrepPhase>();

    }

    // Update is called once per frame
    void Update() {

        int spawn = vals.weaponSpawn;
        int spawn1 = vals.weaponSpawn1;
        int spawn2 = vals.weaponSpawn2;
        int spawn3 = vals.weaponSpawn3;
        int spawn4 = vals.weaponSpawn4;


        if (keepSpawn == true && pChck.inPrep == false) {
            placeItems(spawn, upB);
            placeItems(spawn1, Outside);
            placeItems(spawn2, upA);
            placeItems(spawn3, BackofB);
            placeItems(spawn4, BackofA);

            keepSpawn = false;
        }
    }
    public void placeItems(int spawnI, Transform[] pointz) {
        int indexspawn = Random.Range(0, pointz.Length);
        int mod = Random.Range(0, Pistol.Length);
        Vector3 loc = pointz[indexspawn].transform.position;
        //if(spawnI == 0) {
         //   print("nothing");
        //}
        if (spawnI == 1) {
            //Instantiate(Pistol[mod], loc, Quaternion.identity);
            //print("spawn");
            CmdSpawnPistol(loc,mod);
        } else if (spawnI == 2) {
            //Instantiate(Medic, pointz[indexspawn].transform.position, Quaternion.identity);
            CmdSpawnMedic(loc);
        } else if (spawnI == 3) {
            // Instantiate(Armour, pointz[indexspawn].transform.position, Quaternion.identity);
            CmdSpawnArmour(loc);
        } else {
            return;
        }
    }
    [Command]
    void CmdSpawnPistol(Vector3 point,int num) {
        GameObject Spawner = (GameObject)Instantiate(Pistol[num].gameObject, point, Quaternion.identity) as GameObject;//***
        NetworkServer.Spawn(Spawner);
    }
    [Command]
    void CmdSpawnMedic(Vector3 point) {
        GameObject Spawner = (GameObject)Instantiate(Medic, point, Quaternion.identity) as GameObject;//***
        NetworkServer.Spawn(Spawner);
    }
    [Command]
    void CmdSpawnArmour(Vector3 point) {
        GameObject Spawner = (GameObject)Instantiate(Armour, point, Quaternion.identity) as GameObject;//***
        NetworkServer.Spawn(Spawner);
    }
}

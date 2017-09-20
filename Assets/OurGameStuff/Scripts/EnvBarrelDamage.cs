using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvBarrelDamage : MonoBehaviour {

    private const int DAMAGE_AMOUNT = 80;
    // private bool sendMessage = false;
    public bool barrelHasBeenDestoryed = false;
    private bool damageOnce = false;
    //List<GameObject> playersInside = new List<GameObject>();
    private GameObject Variables;
    private VariablesScript ManagerGet;
    private GameObject manager;
    private PlayerManager playerList;
    private const float MAX_DISTANCE = 5.0f;
    private int playerDamageFrom;

    // Use this for initialization
    void Start() {
        Variables = GameObject.FindWithTag("Start");
        ManagerGet = Variables.GetComponent<VariablesScript>();
        manager = ManagerGet.variables;
        playerList = manager.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update() {
        if (damageOnce) {
            return;
        }
        if (barrelHasBeenDestoryed) {
            for (int i = 0; i < playerList.Players.Count; i++) {
                float distance = Vector3.Distance(this.transform.position, playerList.Players[i].transform.position);
                if (distance < MAX_DISTANCE) {
                    prepareAction(playerList.Players[i]);
                }
            }
            damageOnce = true;
        }
        /*
        if (barrelHasBeenDestoryed && sendMessage) {
            for (int i = 0; i < playersInside.Count; i++) {
                Action(playersInside[i]);
            }
            damageOnce = true;
        }
        */
    }
    /*
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            sendMessage = true;
            playersInside.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            sendMessage = false;
            playersInside.Remove(other.gameObject);
        }
    }
    */
    public void damageFrom(int num) {
        playerDamageFrom = num;
    }

    void prepareAction(GameObject player) {
        int[] prep = new int[2];
        prep[0] = DAMAGE_AMOUNT;
        prep[1] = playerDamageFrom;
        Action(player, prep);
    }


    void Action(GameObject other, int[] input) {
        other.transform.SendMessage("BarrelDamage", input);
    }

    /*
           
    */

}

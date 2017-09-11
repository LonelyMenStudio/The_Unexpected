using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvBarrelDamage : MonoBehaviour {

    public int damageAmount = 80;
    private bool sendMessage = false;
    public bool barrelHasBeenDestoryed = false;
    private bool damageOnce = false;
    List<GameObject> playersInside = new List<GameObject>();

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (damageOnce) {
            return;
        }
        if (barrelHasBeenDestoryed && sendMessage) {
            for (int i = 0; i < playersInside.Count; i++) {
                Action(playersInside[i]);
            }
            damageOnce = true;
        }
    }

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

    void Action(GameObject other) {
        other.transform.SendMessage("BarrelDamage", damageAmount);
    }

}

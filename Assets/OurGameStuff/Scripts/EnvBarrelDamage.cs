using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvBarrelDamage : MonoBehaviour {

    public int damageAmount = 80;
    private bool sendMessage = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            sendMessage = true;
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            sendMessage = false;
        }
    }

    void Action(Collider other) {
        other.transform.SendMessage("BarrelDamage", damageAmount);
    }

}

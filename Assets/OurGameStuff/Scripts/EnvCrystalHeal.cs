using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvCrystalHeal : MonoBehaviour {

    public int healAmount = 80;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            other.transform.SendMessage("CrystalHeal", healAmount);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterScript : MonoBehaviour {

    public GameObject[] spawn;
    public AudioSource TeleportSE;


    void OnTriggerEnter ( Collider other) {
        int indexspawn = Random.Range(0, spawn.Length);
        if (other.gameObject.tag == "Player") {
            other.transform.position = spawn[indexspawn].transform.position;
            TeleportSE.Play();

        }
    }
}

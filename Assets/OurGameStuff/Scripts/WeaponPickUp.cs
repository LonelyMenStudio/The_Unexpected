using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour {

    public GameObject floorWep;
    public GameObject playerWep;

    // Use this for initialization
    void Start () {
        playerWep.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player"){
            playerWep.SetActive(true);
            floorWep.SetActive(false);
        }
    }
}

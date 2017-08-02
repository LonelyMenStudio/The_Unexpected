using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deathscreen : MonoBehaviour {
    public GameObject death;
    public GameObject player;
    public GameObject cam;
    public GameObject playerUI;
   
	// Use this for initialization
	void Start () {
        playerUI.SetActive(true);
        death.SetActive(false);
        player.SetActive(true);
      /*  FirstPersonController playerCS = player.gameObject.GetComponent<FirstPersonController>();
        playerCS.enabled = false;*/
	}
	
	// Update is called once per frame
	void Update () { 
        //deathscreen();
    }

   /* void deathscreen() {
        
        bool Chplayerdeath = PlayerStats.isdead;
        if (Chplayerdeath == true) {
            //player.gameObject.GetComponent<FirstPersonController>().enabled = false;
            player.SetActive(false);
            cam.SetActive(true);
            death.SetActive(true);
            playerUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }*/
}

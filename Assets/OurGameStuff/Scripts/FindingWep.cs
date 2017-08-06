using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindingWep : MonoBehaviour {
    //Not working in muilt atm trying to think how to reference it to the weapon, may end up moving everything to another script
    public float distance; // Distance from the assigned wep
    public GameObject target; //This is the players assigned weapon
    public AudioSource[] sounds;
    AudioSource Beepsound; //Sound for the player to know how close to the wep they are.
    private float Beeping = 2.0f;
    public bool radarsound = true;
    public Image Radar;
    void Start() {
        
        Beepsound = sounds[0];
    }

    void Update() {
        target = GameObject.FindWithTag("Test");
        distance = Vector3.Distance(transform.position, target.transform.position);
        Beeping = distance / 20;
        if (radarsound == true) {

            radarsound = false;
            StartCoroutine(Beep());
        }
    }

      IEnumerator Beep() {
        Beepsound.Play();
        //StartCoroutine(flash());
        yield return new WaitForSeconds(Beeping);
        radarsound = true;

  }
   /* IEnumerator flash() {
        Radar.fillAmount = 1;
        yield return new WaitForSeconds(1);
        Radar.fillAmount = 0;


    }*/
}

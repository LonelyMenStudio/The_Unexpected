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
    public GameObject Canvas;

    void Start() {
        Canvas = GameObject.FindWithTag("Radar pulse");
        Beepsound = sounds[0];
        Radar = Canvas.GetComponent<Image>();
        target = GameObject.FindWithTag("Test");
    }

    void Update() {
        
        distance = Vector3.Distance(transform.position, target.transform.position);
        Beeping = distance / 30;
        if (radarsound == true) {
            Radar.fillAmount = 1 - (distance/300);
            radarsound = false;
            StartCoroutine(Beep());
        }
    }

      IEnumerator Beep() {
        Beepsound.Play();
        yield return new WaitForSeconds(Beeping);
        radarsound = true;
  }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FindingWep : NetworkBehaviour {
    //Not working in muilt atm trying to think how to reference it to the weapon, may end up moving everything to another script
    public float distance; // Distance from the assigned wep
    public PlayerManager target; //This is the players assigned weapon
    public AudioSource[] sounds;
    AudioSource Beepsound; //Sound for the player to know how close to the wep they are.
    private float Beeping = 2.0f;
    public bool radarsound = true;
    public Image Radar;
    public GameObject Canvas;
    private GameObject Variables;
    private VariablesScript ManagerGet;
    private GameObject Manager;
    private PlayerManager pManager;
    private PlayerAssignGet player;
    private int playerno;
    private List<GameObject> droppedWeps;

    void Start() {
        Canvas = GameObject.FindWithTag("Radar pulse");
        Beepsound = sounds[0];
        Radar = Canvas.GetComponent<Image>();
        Variables = GameObject.FindWithTag("Start");
        ManagerGet = Variables.GetComponent<VariablesScript>();
        Manager = ManagerGet.variables;
        pManager = Manager.GetComponent<PlayerManager>();
        player = this.gameObject.GetComponent<PlayerAssignGet>();
        playerno = player.currentPlayerNo;
        droppedWeps = pManager.droppedWeapons;
    }

    void Update() {
        if (!isLocalPlayer) {
            return;
        }
       // for (int i = 1; i < pManager.Players.Count; i++) {
       foreach(GameObject weapon in droppedWeps){
            weaponSettings weaponPlayerCheck = weapon.GetComponent<weaponSettings>();
            if(playerno == weaponPlayerCheck.playerNo) {
                distance = Vector3.Distance(transform.position, weapon.transform.position);
                Beeping = distance / 30;
                if (radarsound == true) {
                    Radar.fillAmount = 1 - (distance / 300);
                    radarsound = false;
                    StartCoroutine(Beep());
                }
            }
        }
            
            
        
        
    }

      IEnumerator Beep() {
        Beepsound.Play();
        yield return new WaitForSeconds(Beeping);
        radarsound = true;
  }

}

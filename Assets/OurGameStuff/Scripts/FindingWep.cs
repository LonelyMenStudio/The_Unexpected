using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FindingWep : NetworkBehaviour {
    //Not working in muilt atm trying to think how to reference it to the weapon, may end up moving everything to another script
    public float distance; // Distance from the assigned wep
    public float distance2;
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
    private List<GameObject> Players;
    private PrepPhase gObject;
    

    void Start() {

        
        Canvas = GameObject.FindWithTag("Radar pulse");
        Beepsound = sounds[0];
        
        Variables = GameObject.FindWithTag("Start");
        ManagerGet = Variables.GetComponent<VariablesScript>();
        Manager = ManagerGet.variables;
        gObject = Manager.GetComponent<PrepPhase>();
        pManager = Manager.GetComponent<PlayerManager>();
        player = this.gameObject.GetComponent<PlayerAssignGet>();
        playerno = player.currentPlayerNo;
        droppedWeps = pManager.droppedWeapons;
        Players = pManager.Players;
        


        if (!isLocalPlayer) {
            return;
        }
        Radar = Canvas.GetComponent<Image>();
        Canvas.SetActive(false);
        //put references
    }

    void Update() {
        if (!isLocalPlayer) {
            return;
        }
        pManager.droppedWeapons.RemoveAll(item => item == null);
        if (gObject.inPrep == false) {
            Canvas.SetActive(true);
            // for (int i = 1; i < pManager.Players.Count; i++) {
            foreach (GameObject weapon in droppedWeps) {
                if (weapon != null) {
                    weaponSettings weaponPlayerCheck = weapon.GetComponent<weaponSettings>();
                    if (playerno == weaponPlayerCheck.playerNo) {
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
           /* foreach (GameObject EPlayer in Players) {
                    PlayerAssignGet PlayerNum = EPlayer.GetComponent<PlayerAssignGet>();
                    if (playerno != PlayerNum.currentPlayerNo) {
                        weaponManager EnemyHasWep = EPlayer.GetComponent<weaponManager>();
                        if (playerno == EnemyHasWep.currentWeaponPlayer) {
                            distance2 = Vector3.Distance(transform.position, EPlayer.transform.position);
                            Beeping = distance2 / 30;
                            if (radarsound == true) {
                                Radar.fillAmount = 1 - (distance2 / 300);
                                radarsound = false;
                                StartCoroutine(Beep());
                            }
                        }
                    }
            }*/
        }
            
            
        
        
    }

      IEnumerator Beep() {
        Beepsound.Play();
        yield return new WaitForSeconds(Beeping);
        radarsound = true;
  }

}

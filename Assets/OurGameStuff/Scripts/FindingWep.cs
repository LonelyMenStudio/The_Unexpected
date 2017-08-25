using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FindingWep : NetworkBehaviour {
    
    public float distance; // Distance from the assigned wep
    //public PlayerManager target; //This is the players assigned weapon
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
    public int playerno;
    public List<GameObject> droppedWeps;
     private List<GameObject> Playerz;
    private PrepPhase gObject;
    public Material[] TeamColors;
    private GameObject TeamColor;


    void Start() {


        Canvas = GameObject.FindWithTag("Radar pulse");
        Beepsound = sounds[0];
        Variables = GameObject.FindWithTag("Start");
        ManagerGet = Variables.GetComponent<VariablesScript>();
        Manager = ManagerGet.variables;
        gObject = Manager.GetComponent<PrepPhase>();
        pManager = Manager.GetComponent<PlayerManager>();
        droppedWeps = pManager.droppedWeapons;
        Playerz = pManager.Players;
        player = this.gameObject.GetComponent<PlayerAssignGet>();
        playerno = player.currentPlayerNo;
        TeamColor = this.gameObject.transform.GetChild(3).gameObject;
        TeamColor.GetComponent<Renderer>().material = TeamColors[playerno];
        if (!isLocalPlayer) {
            return;
        }
        Radar = Canvas.GetComponent<Image>();
        Canvas.SetActive(false);
    }
    void Update() {
        
        if (!isLocalPlayer) {
            return;
        }
       
        Playerz.RemoveAll(item => item == null);
        pManager.droppedWeapons.RemoveAll(item => item == null);
        if (gObject.inPrep == false) {
            Canvas.SetActive(true);
            foreach (GameObject weapon in droppedWeps) {
                if (weapon != null) {
                    player = this.gameObject.GetComponent<PlayerAssignGet>();
                    playerno = player.currentPlayerNo;
                    weaponSettings weaponPlayerCheck = weapon.GetComponent<weaponSettings>();
                    if (playerno == weaponPlayerCheck.playerNo) {
                        distanceCheck(weapon);

                    }
                }
            }

            foreach (GameObject EPlayer in Playerz) {
                if (EPlayer != null) {
                    PlayerAssignGet PlayerNum = EPlayer.GetComponent<PlayerAssignGet>();
                    if (playerno != PlayerNum.currentPlayerNo) {
                        player = this.gameObject.GetComponent<PlayerAssignGet>();
                        playerno = player.currentPlayerNo;
                        weaponManager EnemyHasWep = EPlayer.GetComponent<weaponManager>();
                        if (playerno == EnemyHasWep.currentWeaponPlayer) {
                            distanceCheck(EPlayer);
                        }
                    }
                }
            }
        }
    }
    
void distanceCheck(GameObject target) {
    distance = Vector3.Distance(transform.position, target.transform.position);
    Beeping = distance / 30;
    if (radarsound == true) {
            //Radar.fillAmount = 1 - (distance / 300);
            Radar.transform.localScale = new Vector3(3 *( 1- distance/300),2 * (1- distance / 300), 0);
           // Radar.GetComponent(RectTransform).sizeDelta = new Vector2(100 * (1 - distance / 300), 100 * (1 - distance / 300));
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

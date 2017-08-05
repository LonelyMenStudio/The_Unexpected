using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Playeranimations : NetworkBehaviour {

    Animator animatorz;
    public GameObject Rig;
    private bool isCrounched = false;
    private bool jump = false;
    private GameObject manager;
    public GameObject wep;
    private PrepPhase ph;
    private weaponManager wloss;
    private bool haswep = false;
    private bool lossWep = false;
    private bool Aim = false;
    public GameObject player;
    public GameObject Variables;
    private VariablesScript ManagerGet;

    void Awake() {
        Variables = GameObject.FindWithTag("Start");
    }

    // Use this for initialization
    void Start () {
        ManagerGet = Variables.GetComponent<VariablesScript>();
        animatorz = Rig.GetComponent<Animator>();
        manager = ManagerGet.variables;
        ph = manager.GetComponent<PrepPhase>();
        //wep = GameObject.FindWithTag("Player");
        wloss = gameObject.GetComponent<weaponManager>();
    }
	
	// Update is called once per frame
	void Update () {
         if (!isLocalPlayer) { //what is this script on???
         return;
          }
        haswep = wloss.hasWeapon;
        lossWep = ph.playwep;
        bool isWalkingPressed = Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftShift) ;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);
        
        animatorz.SetBool("isWalking", isWalkingPressed);
        animatorz.SetBool("HasWep", haswep);
        animatorz.SetBool("Sprint", isRunning);
        
        if (lossWep == true) {
            animatorz.Play("GunLost");
            ph.playwep = false;
        }
        else if (Input.GetKey(KeyCode.C) && isCrounched == true) {
            animatorz.SetBool("Crounched", false);
            isCrounched = false;
        } 
        else if (Input.GetMouseButtonDown(1) && Aim == false) {
            animatorz.SetBool("Aim", true);
            Aim = true;
        }
        else if (Input.GetMouseButtonDown(1) && Aim == true) {
            animatorz.SetBool("Aim", false);
            Aim = false;
        }
        else if (Input.GetKey(KeyCode.Space) && haswep == false) {
            animatorz.Play("JumpNoGun");
        }
        else if (Input.GetKey(KeyCode.Space) && haswep == true) {
            animatorz.Play("JumpWithGun");
        }
       else if (Input.GetKey(KeyCode.C) && isCrounched == false) {
            animatorz.SetBool("Crounched", true);
            isCrounched = true;
        }
            else {
            //needed to make the last else if work
        }
    }
}

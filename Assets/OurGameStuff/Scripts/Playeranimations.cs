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
    public bool Aim = false;
    public GameObject player;
    public GameObject Variables;
    private VariablesScript ManagerGet;
    private Health isDead;
    public bool reloading = true;
    private const float RELOAD_TIME = 3.0f;
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
        isDead = gameObject.GetComponent<Health>();
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
        bool StraftRight = Input.GetKey(KeyCode.D);
        bool StraftLeft = Input.GetKey(KeyCode.A);
        
        animatorz.SetBool("isWalking", isWalkingPressed);
        animatorz.SetBool("HasWep", haswep);
        animatorz.SetBool("Sprint", isRunning);
        animatorz.SetBool("StraftRight", StraftRight);
        animatorz.SetBool("StraftLeft", StraftLeft);


        if ( Input.GetKey(KeyCode.R) && reloading && haswep) {
            reloading = false;
            animatorz.Play("Reload");
            StartCoroutine(Reload());
        }
        if (Input.GetKey(KeyCode.Space)) {
            Aim = false;
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            Aim = false;
        }
        if ( isDead.death == true) {
            animatorz.Play("Death");
            isDead.death = false;
        }
        if (lossWep == true) {
            animatorz.Play("GunLost");
            ph.playwep = false;
        }

        if (Input.GetMouseButtonDown(1) && Aim == false) {
            animatorz.SetBool("Aim", true);
            Aim = true;
        } else if (Input.GetMouseButtonDown(1) && Aim == true) {
            animatorz.SetBool("Aim", false);
            Aim = false;
        }

        if (Input.GetKey(KeyCode.Space) && haswep == false) {
            animatorz.Play("JumpNoGun");
        } else if (Input.GetKey(KeyCode.Space) && haswep == true) {
            animatorz.Play("JumpWithGun");
        }
     
    }
    IEnumerator Reload() {
        //reload.Play();
        //transform.Find("crystal").gameObject.GetComponent<Animation>().Play("Take 001");
        yield return new WaitForSeconds(RELOAD_TIME);
        reloading = true;

    }
}

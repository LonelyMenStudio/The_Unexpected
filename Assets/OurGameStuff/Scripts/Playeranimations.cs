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
    private GameObject scope;
    public GameObject Scopecam;
    private int wepout;
    public Camera MainCam;
    private float normalFOV;
    public float scopedFOV = 15f;
    public bool Change = false;
    private IKControl resetlerp;
    private bool aiming;
    void Awake() {
        Variables = GameObject.FindWithTag("Start");
    }

    // Use this for initialization
    void Start() {
        //Scopecam.SetActive(false);
        ManagerGet = Variables.GetComponent<VariablesScript>();
        animatorz = Rig.GetComponent<Animator>();
        manager = ManagerGet.variables;
        ph = manager.GetComponent<PrepPhase>();
        //wep = GameObject.FindWithTag("Player");
        wloss = gameObject.GetComponent<weaponManager>();
        isDead = gameObject.GetComponent<Health>();
        resetlerp = gameObject.GetComponent<IKControl>();
        // scope = ph.Scopein;
        if (!isLocalPlayer) {
            return;
        }
        //Scopecam.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer) { //what is this script on???
            return;
        }
        haswep = wloss.hasWeapon;
        lossWep = ph.playwep;
        wepout = wloss.weaponOut;
        bool isWalkingPressed = Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftShift);
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);
        bool StraftRight = Input.GetKey(KeyCode.D);
        bool StraftLeft = Input.GetKey(KeyCode.A);
        bool Backward = Input.GetKey(KeyCode.S);

        animatorz.SetBool("isWalking", isWalkingPressed);
        animatorz.SetBool("HasWep", haswep);
        animatorz.SetBool("Sprint", isRunning);
        animatorz.SetBool("StraftRight", StraftRight);
        animatorz.SetBool("StraftLeft", StraftLeft);
        animatorz.SetBool("Backwards", Backward);

        if (Input.GetKey(KeyCode.R) && reloading && haswep) {
            aiming = Aim;
            if (aiming) {
                Aim = false;
            }
            resetlerp.currentlerp = 0;
            ReloadAnim();
        }
        if (Input.GetKey(KeyCode.Space)) {
            Aim = false;
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            Aim = false;
        }
        if (isDead.death == true) {
            animatorz.Play("Death");
            isDead.death = false;
        }
        if (lossWep == true) {
            animatorz.Play("GunLost");
            ph.playwep = false;
        }
        if (!wloss.hasWeapon) {
            Aim = false;
        }
        if (Input.GetMouseButtonDown(1)) {
            Change = true;
            Aim = !Aim;
            animatorz.SetBool("Aim", Aim);
            if (wepout == 3) {
                /*  if (Aim)
                      StartCoroutine(OnScoped());
                  else
                      OnUnScoped();*/
            }
            if (wepout == 0) {
                Aim = false;
            }
        }

        if (Input.GetKey(KeyCode.Space) && haswep == false) {
            animatorz.Play("JumpNoGun");
        } else if (Input.GetKey(KeyCode.Space) && haswep == true) {
            animatorz.Play("JumpWithGun");
        }

    }
    /*  void OnUnScoped() {
          scope.SetActive(false);
          Scopecam.SetActive(true);
          MainCam.fieldOfView = normalFOV;
      }*/

    public void ReloadAnim() {
        reloading = false;
        animatorz.Play("Reload");
        StartCoroutine(Reload());
    }
    public void TakeAim() {
        Change = true;
        // Aim = true;
    }
    IEnumerator Reload() {
        //reload.Play();
        //transform.Find("crystal").gameObject.GetComponent<Animation>().Play("Take 001");
        yield return new WaitForSeconds(RELOAD_TIME);
        reloading = true;
        if (aiming) {
            Aim = true;
            Change = true;
        }

    }
    /* IEnumerator OnScoped() {
         yield return new WaitForSeconds(.15f);
         scope.SetActive(true);
        Scopecam.SetActive(false);

         normalFOV = MainCam.fieldOfView;
        /MainCam.fieldOfView = scopedFOV;
     }*/
}

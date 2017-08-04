using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class weaponManager : NetworkBehaviour {

    //weapon names are currently just placeholders, marked are the locations of the placholder usage

    public GameObject pistol;
    public GameObject ak;
    private int weaponOut = 0;//1 ak, 2 pistol
    private int secondWeapon = 0;
    public bool hasWeapon = false;
    public int currentWeaponAmmo;
    public int currentWeaponMaxAmmo;
    private int currentWeaponDamage = 10;
    public GameObject childMelee;
    public GameObject childWeapon1;
    public GameObject childWeapon2;
    public GameObject body;
    private GameObject childRoot;
    private GameObject weaponDropperTemp;
    public GameObject ammoDisplay;
    private Text ammoText;
    private GameObject prepHud;
    private PrepPhase gObject;
     AudioSource fire;
     AudioSource reload;
    private float delayTime = 0.05f;
    private float counter = 0.0f;
    private bool canShoot = false;
    public GameObject bulletHole;
    public AudioSource[] sounds;
    private bool spawnhole = true;
    public float distance; // Distance from the assigned wep
    public GameObject target; //This is the players assigned weapon
//<<<<<<< HEAD

    private PlayerAssignGet pl;
    public int currentPlayer;
    private int currentWeaponPlayer;
    private bool inWeaponSelect = true;//true when timer done // hopefully done
    private bool selectionDone = false;
    private int weaponOnEnd = 0;//0=ak,
    private GameObject[] weaponRespawnLocation;
    private GameObject manager;
    private PlayerAssign wrl;
    private PlayerManagerSelf playerManage;
    private bool checkingPrep = true;

//=======
    AudioSource Beepsound; //Sound for the player to know how close to the wep they are.
    
//>>>>>>> c3943934d6b06f638b2c283b56224c49b4642929
    //Animator animatorz;

    private const float RELOAD_TIME = 2.0f;

    void Awake() {
        sounds = GetComponents<AudioSource>();
        fire = sounds[1];
       // Beepsound = sounds[2];
       // reload = sounds[0];
        //animatorz = GetComponent<Animator>();
        prepHud = GameObject.Find("Manager");//not called manager?
        gObject = prepHud.GetComponent<PrepPhase>();
        ammoDisplay = gObject.ammoObject;
        ammoText = ammoDisplay.GetComponent<Text>();
        manager = GameObject.Find("Managerz");
        wrl = manager.GetComponent<PlayerAssign>();
        playerManage = this.gameObject.GetComponent<PlayerManagerSelf>();

    }

    // Use this for initialization
    void Start() {
        weaponRespawnLocation = new GameObject[8];
        for (int i = 0; i < 8; i++) {
            weaponRespawnLocation[i] = wrl.weaponRespawnPoints[i];
        }
        if (isLocalPlayer) {
            body.layer = 2;
        }
        childRoot = transform.Find("FirstPersonCharacter").gameObject;
        pl = this.gameObject.GetComponent<PlayerAssignGet>();
        //currentPlayer = pl.currentPlayerNo;//activating too early need to make it on first action this is needed
    }

    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer) {
            return;
        }
        //distance = Vector3.Distance(transform.position, target.position);
       // currentPlayer = pl.currentPlayerNo;//remove when weapon select is enabled
        if (checkingPrep && !gObject.inPrep) {
            inWeaponSelect = false;
            selectionDone = true;
            checkingPrep = false;
        }
        if (inWeaponSelect) {
            currentPlayer = pl.currentPlayerNo;//should be good here
            if (Input.GetKeyDown(KeyCode.E)) {  // need to investigate further control options
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, childRoot.transform.forward, out hit)) {
                    if (hit.transform.tag == "weaponPrep") {
                        if (hit.transform.gameObject.name.Contains("PrepAk")) { //ony going for ak atm
                            weaponOnEnd = 0;
                            changeWeapon(1);
                        }
                    }
                }
            }
            return;//cancels out of rest of program if is in prep
        }
        if (selectionDone) {
            if (weaponOnEnd == 0) {
                loseWeapon();
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && hasWeapon && canShoot && counter > delayTime) { // probs can be cut down to only 1 raycast
            shoot();
            fire.Play();
        }
        counter += Time.deltaTime;//counter to ensure not infinite fire rate
        if (Input.GetKeyDown(KeyCode.R) && hasWeapon) {
            StartCoroutine(Reload());
        }
        if (Input.GetKeyDown(KeyCode.E)) {  // need to investigate further control options
            PickupWeapon();
        }
        if (Input.GetKeyDown(KeyCode.F) && hasWeapon) {//used to be switch weapon but removing it
            dropWeapon();
        }
        //ammoText.text = currentWeaponAmmo + "/" +currentWeaponMaxAmmo;
        if(currentWeaponAmmo <= 0) {
            canShoot = false;
        }
        /* if (distance > 150) {
             sounds[2].Play();
             StartCoroutine(Beeping(4));
         } else if ( distance <= 150 && distance > 50){
             sounds[2].Play();
             StartCoroutine(Beeping(3));
         */
    }

    void PickupWeapon() {
        if (weaponOut == 0 && hasWeapon) {
            switchWeapon();
        }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, childRoot.transform.forward, out hit)) {
            if (hit.transform.tag == "weapon") {
                if (hit.transform.gameObject.name.Contains("crystal")) { //***
                    replaceWeapon(hit);
                    changeWeapon(1);
                }
                if (hit.transform.gameObject.name.Contains("ak")) {//***
                    replaceWeapon(hit);
                    changeWeapon(2);
                    }

            }
        }
    }

    void changeWeapon(int num) {
        if (!isServer) {
            CmdSwitchWeapon(num);
        } else {
            RpcSwitchWeapon(num);
        }
    }

    [Command]
    void CmdSwitchWeapon(int weapon) {
        RpcSwitchWeapon(weapon);
    }
    [ClientRpc]
    void RpcSwitchWeapon(int weapon) {
        if (weapon == 0) {
            weaponOut = 0;
            /*
            transform.FindChild("knife").gameObject.SetActive(true);//***
            transform.FindChild("ak").gameObject.SetActive(false);//***
            transform.FindChild("pistol").gameObject.SetActive(false);//***
            */
            childMelee.SetActive(true);//***
            childWeapon1.SetActive(false);//***
            childWeapon2.SetActive(false);//***
        }
        if (weapon == 1) {
            weaponOut = 1;
            childMelee.SetActive(false);//***
            childWeapon1.SetActive(true);//***
            childWeapon2.SetActive(false);//***
        } else if (weapon == 2) {
            weaponOut = 2;
            childMelee.SetActive(false);//***
            childWeapon1.SetActive(false);//***
            childWeapon2.SetActive(true);//***
        }
    }

    /*
    void ChangeWeapon(int weapon) {
        if (weapon == 0) {
            weaponOut = 0;
            
            childMelee.SetActive(true);//***
            childWeapon1.SetActive(false);//***
            childWeapon2.SetActive(false);//***
        }
        if (weapon == 1) {
            weaponOut = 1;
            childMelee.SetActive(false);//***
            childWeapon1.SetActive(true);//***
            childWeapon2.SetActive(false);//***
        } else if (weapon == 2) {
            weaponOut = 2;
            childMelee.SetActive(false);//***
            childWeapon1.SetActive(false);//***
            childWeapon2.SetActive(true);//***
        }
    }
    */
    private void RespawnAK() {
        CmdRespawnAK();
        playerManage.AddWeaponToList(weaponDropperTemp);
        CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
    }
    private void spawnAK() {
        CmdSpawnAK();
        playerManage.AddWeaponToList(weaponDropperTemp);
       
    }
    private void destoryWeapon(GameObject destoryThis) {
        playerManage.DropWeaponFromList(destoryThis);
        CmdDestroyHit(destoryThis);
    }
    private void spawnPistol() {
        //???
    }


    [Command]
    void CmdRespawnAK() {
        GameObject weaponDropper = (GameObject)Instantiate(ak, weaponRespawnLocation[Random.Range(0, 7)].transform.position, Quaternion.identity) as GameObject;//***
        NetworkServer.Spawn(weaponDropper);
        weaponDropperTemp = weaponDropper;
    }

    [Command]
    void CmdSpawnAK() {
        GameObject weaponDropper = (GameObject)Instantiate(ak, transform.root.transform.position + new Vector3(0, 1, 0) + transform.forward, Quaternion.identity) as GameObject;//***
        NetworkServer.Spawn(weaponDropper);
        weaponDropperTemp = weaponDropper;
    }
    [Command]
    void CmdSpawnPistol() {
        GameObject weaponDropper = (GameObject)Instantiate(pistol, transform.root.transform.position + new Vector3(0, 1, 0) + transform.forward * 2, Quaternion.identity) as GameObject;//***
        NetworkServer.Spawn(weaponDropper);
        weaponDropperTemp = weaponDropper;
    }
    [Command]
    void CmdDestroyHit(GameObject objectToDestory) {
        NetworkServer.Destroy(objectToDestory.transform.gameObject);
    }

    void loseWeapon() {
        hasWeapon = false;
        canShoot = false;
        if (!isServer) {
            CmdSwitchWeapon(0);
        } else {
            RpcSwitchWeapon(0);
        }
        RespawnAK();
        //weapon type?
        //possibly clear out ammo from weapon
        //clear any other weapon effects if added
    }
    void dropWeapon() {
        hasWeapon = false;
        canShoot = false;
        if (!isServer) {
            CmdSwitchWeapon(0);
        } else {
            RpcSwitchWeapon(0);
        }
            if (weaponOut == 1) {
                spawnAK();
                CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
            } else if (weaponOut == 2) {
                CmdSpawnPistol();
                CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
            }
    }

    void replaceWeapon(RaycastHit hit) {
        if (!hasWeapon) {
            weaponAmmoPick(hit);
            childMelee.SetActive(false);
            hasWeapon = true;
            canShoot = true;
            return;
        } else {
            if (weaponOut == 1) {
                spawnAK();
                CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
            } else if (weaponOut == 2) {
                CmdSpawnPistol();
                CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
            }
            weaponAmmoPick(hit);
        }
    }
    void switchWeapon() {
        canShoot = !canShoot;
        int temp = weaponOut;
        //ChangeWeapon(secondWeapon);
        if (!isServer) {
            CmdSwitchWeapon(secondWeapon);
        } else {
            RpcSwitchWeapon(secondWeapon);
        }
        secondWeapon = temp;
    }

    void deathReset() {
        //palyer respawn
        loseWeapon();
    }


    void weaponAmmoPick(RaycastHit hitWeapon) {
        GameObject weaponPicker = hitWeapon.collider.gameObject;
        weaponSettings currentWeaponSettingsGet = weaponPicker.GetComponent<weaponSettings>();
        currentWeaponAmmo = currentWeaponSettingsGet.currentAmmo;
        currentWeaponMaxAmmo = currentWeaponSettingsGet.maxAmmo;
        // NetworkServer.Destroy(hit.transform.gameObject);
        currentWeaponPlayer = currentWeaponSettingsGet.playerNo;
        CmdDestroyHit(weaponPicker);
        
    }

    [Command]
    void CmdWeaponAmmoDrop(GameObject weaponDropper, int inputC, int inputM, int inputP) {
        weaponSettings currentWeaponSettings = weaponDropperTemp.GetComponent<weaponSettings>();
        currentWeaponSettings.currentAmmo = inputC;
        currentWeaponSettings.maxAmmo = inputM;
        currentWeaponSettings.playerNo = inputP;
        weaponDropperTemp = null;
    }

    void shoot() {
        //fire.Play();
        currentWeaponAmmo--;
        counter = 0;
        RaycastHit hit2;
        if (Physics.Raycast(Camera.main.transform.position, childRoot.transform.forward, out hit2)) {
            if (hit2.transform.tag == "Player") {
                CmdDamageDealer(hit2.transform.gameObject, currentWeaponDamage);
                spawnhole = false;
                //Enemy enemyhealth = hit2.collider.gameObject.GetComponent<Enemy>();
                //enemyhealth.TakeDamage(10);  // always sending consistent damage, this will have to be pulled from
            }


        }
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, childRoot.transform.forward);
        if (Physics.Raycast(ray, out hit, 100f) && spawnhole) {
            Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        }
        spawnhole = true;
    }


    void CheckWeaponNumber(int playerNum) {
        if (!isServer) {
            return;
        }
        if (currentWeaponPlayer == playerNum) {
            loseWeapon();//weapon information??
        }
    }

    [Command]
    void CmdDamageDealer(GameObject hit,  int damage) {
        hit.SendMessage("TakeDamage", currentWeaponDamage);
    }

    IEnumerator Reload() {
        //reload.Play();
        canShoot = false;
        //transform.Find("crystal").gameObject.GetComponent<Animation>().Play("Take 001");
        yield return new WaitForSeconds(RELOAD_TIME);
        currentWeaponAmmo = currentWeaponMaxAmmo;
        
        canShoot = true;
        
    }

  /*  IEnumerator Beeping(int BeepSpeed) {
        yield return new WaitForSeconds(BeepSpeed);
    }*/

    
}

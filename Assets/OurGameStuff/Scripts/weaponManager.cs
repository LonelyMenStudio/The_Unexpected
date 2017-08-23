using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class weaponManager : NetworkBehaviour {

    //weapon names are currently just placeholders, marked are the locations of the placholder usage

    public GameObject shotty;
    public GameObject ak;
    public GameObject Sniper;
    private int weaponOut = 0;//1 ak, 2 pistol
    private int secondWeapon = 0;
    public bool hasWeapon = false;
    public int currentWeaponAmmo;
    public int currentWeaponMaxAmmo;
    private float AkDamage = 10;
    private float ShotgunDmg = 15;
    private float sniperDmg = 100;
    public GameObject childMelee;
    public GameObject childWeapon1;
    public GameObject childWeapon2;
    public GameObject childWeapon3;
    public GameObject body;
    private GameObject childRoot;
    private GameObject weaponDropperTemp;
    public GameObject ammoDisplay;
    public Image Hitmark;
    private GameObject HitMarker;
    private Text ammoText;
    public GameObject prepHud;
    private PrepPhase gObject;
    public AudioSource fire;
    public AudioSource reload;
    public AudioSource HitSE;
    public AudioSource Sniper_shot;
    public AudioSource Shotgun_shot;
    private float delayTime = 0.05f;
    private float counter = 0.0f;
    private bool canShoot = false;
    public GameObject bulletHole;
    public GameObject bulletHole1;
    public GameObject Hole;
    public GameObject bulletHoleS;
    //public GameObject bulletHoleSG;
    public GameObject bulletHoleSG1;
    //public GameObject bulletHoleS;
    public GameObject bulletHoleS1;
    public ParticleSystem ShotParticle;
    public ParticleSystem ShotFlash;
    public ParticleSystem ShotGunParticle;
    public ParticleSystem SniperParticle;
    // public AudioSource[] sounds;
    public GameObject AmmoObject;
    private bool spawnhole = true;
    public int Shotgunshells = 6;
    public float ShotgunSpread = 10.0f;
    private bool inReload = false;
    private PlayerAssignGet pl;
    public int currentPlayer;
    public bool hasDroppedOne = false;

    [SyncVar]
    public int currentWeaponPlayer;
    [SyncVar]
    public bool dropIt = false;

    private bool inWeaponSelect = true;//true when timer done // hopefully done
    private bool selectionDone = false;
    private int weaponOnEnd = 0;//0=ak,
    private GameObject[] weaponRespawnLocation;
    private GameObject manager;
    private PlayerAssign wrl;
    private PlayerManagerSelf playerManage;
    private bool checkingPrep = true;
    private bool actionOnce = true;
    public GameObject Variables;
    private VariablesScript ManagerGet;
    private PlayerManager plrMngr;

    //=======


    //>>>>>>> c3943934d6b06f638b2c283b56224c49b4642929
    //Animator animatorz;

    private const float RELOAD_TIME = 2.0f;
    private const float hitmarkertime = 1.0f;

    void Awake() {
        Variables = GameObject.FindWithTag("Start");
    }

    // Use this for initialization
    void Start() {

        ManagerGet = Variables.GetComponent<VariablesScript>();
        //sounds = GetComponents<AudioSource>();
        //reload = sounds[0];
        //fire = sounds[1];
        // HitSE = sounds[2];
        //  Sniper_shot = sounds[5];
        //Shotgun_shot = sounds[4];
        // reload = sounds[0];
        //animatorz = GetComponent<Animator>();
        manager = ManagerGet.variables;
        prepHud = ManagerGet.variables;//dupe
        gObject = prepHud.GetComponent<PrepPhase>();
        //HitMarker
        HitMarker = gObject.HitMark;
        Hitmark = HitMarker.GetComponent<Image>();
        HitMarker.SetActive(false);
        AmmoObject = GameObject.FindWithTag("Ammo");
        //ammoDisplay = gObject.ammoObject;
        ammoText = AmmoObject.GetComponent<Text>();

        wrl = manager.GetComponent<PlayerAssign>();
        playerManage = this.gameObject.GetComponent<PlayerManagerSelf>();

        weaponRespawnLocation = new GameObject[wrl.weaponRespawnPoints.Length];
        for (int i = 0; i < wrl.weaponRespawnPoints.Length; i++) {
            weaponRespawnLocation[i] = wrl.weaponRespawnPoints[i];
        }
        if (isLocalPlayer) {
            body.layer = 2;
        }
        childRoot = transform.Find("FirstPersonCharacter").gameObject;
        pl = this.gameObject.GetComponent<PlayerAssignGet>();
        //currentPlayer = pl.currentPlayerNo;//activating too early need to make it on first action this is needed
        plrMngr = manager.GetComponent<PlayerManager>();
    }
    [Command]
    void CmdSetWeaponPlayer(int setTo) {
        currentWeaponPlayer = setTo;
    }

    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer) {
            return;
        }

        if (hasWeapon == false) {
            currentWeaponAmmo = 0;
            currentWeaponMaxAmmo = 0;
            CmdSetWeaponPlayer(0);
        }
        CheckCanShoot();
        if (dropIt == true && !hasDroppedOne) {
            lostWeapon();
        }
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
                        if (hit.transform.gameObject.name.Contains("PrepRifle")) {
                            weaponOnEnd = 0;
                            changeWeapon(1);
                        } else if (hit.transform.gameObject.name.Contains("PrepShotty")) { //just demo weapon
                            weaponOnEnd = 1;
                            changeWeapon(2);
                        } else if (hit.transform.gameObject.name.Contains("PrepSniper")) { //just demo weapon
                            weaponOnEnd = 2;
                            changeWeapon(3);
                        }
                    }
                }
            }
            return;//cancels out of rest of program if is in prep
        }
        if (selectionDone && actionOnce) {
            actionOnce = false;
            weaponFirstSpawn(weaponOnEnd);  // update to hold more than one weapon
        }

        if (Input.GetKey(KeyCode.Mouse0) && hasWeapon && canShoot && counter > delayTime) { // probs can be cut down to only 1 raycast
            shoot();
            //fire.Play();
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
        ammoText.text = currentWeaponAmmo + "/" + currentWeaponMaxAmmo;
        if (currentWeaponAmmo <= 0) {
            canShoot = false;
        }

    }

    void PickupWeapon() {
        if (weaponOut == 0 && hasWeapon) {
            switchWeapon();
        }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, childRoot.transform.forward, out hit)) {
            if (hit.transform.tag == "weapon") {
                if (hit.transform.gameObject.name.Contains("alienrifle")) { //***
                    replaceWeapon(hit);
                    changeWeapon(1);
                }
                if (hit.transform.gameObject.name.Contains("Shotty")) {//***
                    replaceWeapon(hit);
                    changeWeapon(2);
                }
                if (hit.transform.gameObject.name.Contains("AlienSniper")) {//***
                    replaceWeapon(hit);
                    changeWeapon(3);
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
            childWeapon3.SetActive(false);//***
        }
        if (weapon == 1) {
            weaponOut = 1;
            delayTime = 0.05f;
            childMelee.SetActive(false);//***
            childWeapon1.SetActive(true);//***
            childWeapon2.SetActive(false);//***
            childWeapon3.SetActive(false);//***
        } else if (weapon == 2) {
            weaponOut = 2;
            delayTime = 0.3f;
            childMelee.SetActive(false);//***
            childWeapon1.SetActive(false);//***
            childWeapon2.SetActive(true);//***
            childWeapon3.SetActive(false);//***
        } else if (weapon == 3) {
            weaponOut = 3;
            delayTime = 0.8f;
            childMelee.SetActive(false);//***
            childWeapon1.SetActive(false);//***
            childWeapon2.SetActive(false);//***
            childWeapon3.SetActive(true);//***
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
        // playerManage.AddWeaponToList(weaponDropperTemp);
        CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
    }
    private void spawnAK() {
        CmdSpawnAK();
        // playerManage.AddWeaponToList(weaponDropperTemp);

    }


    private void destoryWeapon(GameObject destoryThis) {
        CmdDestroyHit(destoryThis);
    }
    private void spawnPistol() {// need work
        CmdSpawnPistol();
        // playerManage.AddWeaponToList(weaponDropperTemp);
    }
    private void RespawnPistol() {// need work
        CmdRespawnPistol();
        // playerManage.AddWeaponToList(weaponDropperTemp);
        CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
    }
    private void spawnSniper() {// need work
        CmdSpawnPistol();
        // playerManage.AddWeaponToList(weaponDropperTemp);
    }
    private void RespawnSniper() {// need work
        CmdRespawnSniper();
        // playerManage.AddWeaponToList(weaponDropperTemp);
        CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
    }



    [Command]
    void CmdRespawnAK() {
        GameObject weaponDropper = (GameObject)Instantiate(ak, weaponRespawnLocation[Random.Range(0, 7)].transform.position, Quaternion.identity) as GameObject;//***
        NetworkServer.Spawn(weaponDropper);
        weaponDropperTemp = weaponDropper;
    }
    [Command]
    void CmdRespawnPistol() {
        GameObject weaponDropper = (GameObject)Instantiate(shotty, weaponRespawnLocation[Random.Range(0, 7)].transform.position, Quaternion.identity) as GameObject;//***
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
        GameObject weaponDropper = (GameObject)Instantiate(shotty, transform.root.transform.position + new Vector3(0, 1, 0) + transform.forward * 2, Quaternion.identity) as GameObject;//***
        NetworkServer.Spawn(weaponDropper);
        weaponDropperTemp = weaponDropper;
    }
    [Command]
    void CmdRespawnSniper() {
        GameObject weaponDropper = (GameObject)Instantiate(Sniper, weaponRespawnLocation[Random.Range(0, 7)].transform.position, Quaternion.identity) as GameObject;//***
        NetworkServer.Spawn(weaponDropper);
        weaponDropperTemp = weaponDropper;
    }
    [Command]
    void CmdSpawnSniper() {
        GameObject weaponDropper = (GameObject)Instantiate(Sniper, transform.root.transform.position + new Vector3(0, 1, 0) + transform.forward * 2, Quaternion.identity) as GameObject;//***
        NetworkServer.Spawn(weaponDropper);
        weaponDropperTemp = weaponDropper;
    }
    [Command]
    void CmdDestroyHit(GameObject objectToDestory) {
        weaponSettings clearWeapon = objectToDestory.GetComponent<weaponSettings>();
        //clearWeapon.removeSelfFromList(); //NOT NEEDED
        NetworkServer.Destroy(objectToDestory.transform.gameObject);
    }


    void weaponFirstSpawn(int weaponToSpawn) {
        hasWeapon = false;
        canShoot = false;
        if (!isServer) {
            CmdSwitchWeapon(0);
        } else {
            RpcSwitchWeapon(0);
        }
        if (weaponToSpawn == 0) {
            CmdRespawnAK();
        } else if (weaponToSpawn == 1) {
            CmdRespawnPistol();
        } else if (weaponToSpawn == 2) {
            CmdRespawnSniper();
        } else {
            Debug.Log("problem spawning the weapon");
        }
        // playerManage.AddWeaponToList(weaponDropperTemp);
        CmdWeaponAmmoDrop(weaponDropperTemp, 30, 30, currentPlayer);
    }
    /*void weaponFirstSpawn(int weaponToSpawn) {
        hasWeapon = false;
        canShoot = false;
        if (!isServer) {
            CmdSwitchWeapon(0);
        } else {
            RpcSwitchWeapon(0);
        }
        if (weaponToSpawn == 0) {
            CmdRespawnAK();
            CmdWeaponAmmoDrop(weaponDropperTemp, 30, 30, currentPlayer);
        } else if (weaponToSpawn == 1) {
            CmdRespawnPistol();
            CmdWeaponAmmoDrop(weaponDropperTemp, 12, 12, currentPlayer);
        } else if (weaponToSpawn == 2) {
            CmdRespawnSniper();
            CmdWeaponAmmoDrop(weaponDropperTemp, 6, 6, currentPlayer);
        } else {
            Debug.Log("problem spawning the weapon");
        }
        // playerManage.AddWeaponToList(weaponDropperTemp);
        //CmdWeaponAmmoDrop(weaponDropperTemp, 30, 30, currentPlayer);
    }*/
    void loseWeapon() {
        if (!isLocalPlayer) {
            return;
        }
        hasWeapon = false;
        canShoot = false;
        if (!isServer) {
            CmdSwitchWeapon(0);
        } else {
            RpcSwitchWeapon(0);
        }
        if (weaponOut == 1) {
            RespawnAK();
        }
        if (weaponOut == 2) {
            RespawnPistol();
        }
        if (weaponOut == 3) {
            RespawnSniper();
        }


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
        } else if (weaponOut == 3) {
            CmdSpawnSniper();
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
                //CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
            } else if (weaponOut == 2) {
                CmdSpawnPistol();
                CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
            } else if (weaponOut == 3) {
                CmdSpawnSniper();
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

    void weaponAmmoPick(RaycastHit hitWeapon) {
        GameObject weaponPicker = hitWeapon.collider.gameObject;
        weaponSettings currentWeaponSettingsGet = weaponPicker.GetComponent<weaponSettings>();
        currentWeaponAmmo = currentWeaponSettingsGet.currentAmmo;
        currentWeaponMaxAmmo = currentWeaponSettingsGet.maxAmmo;
        // NetworkServer.Destroy(hit.transform.gameObject);
        CmdSetWeaponPlayer(currentWeaponSettingsGet.playerNo);
        //currentWeaponPlayer = currentWeaponSettingsGet.playerNo;
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
        if (weaponOut != 2) {
            currentWeaponAmmo--;
            counter = 0;
        } else if (weaponOut == 2) {
            currentWeaponAmmo -= 6;
            counter = 0;

        } else {

        }
        RaycastHit hit2;
        if (weaponOut == 1 && Physics.Raycast(Camera.main.transform.position, childRoot.transform.forward, out hit2)) {
            if (hit2.transform.tag == "Player") {
                fire.Play();
                HitSE.Play();
                float distance = Vector3.Distance(transform.position, hit2.transform.position);
                if (distance >= 300) {
                    distance = 299;
                }
                AkDamage = AkDamage * 1 - (distance / 300);
                int damageAK = (int)AkDamage;
                CmdDamageDealer(hit2.transform.gameObject, damageAK, currentPlayer);
                spawnhole = false;
                HitMarker.SetActive(true);
                StartCoroutine(hit());
                //HitMarkersound here;
                //Enemy enemyhealth = hit2.collider.gameObject.GetComponent<Enemy>();
                //enemyhealth.TakeDamage(10);  // always sending consistent damage, this will have to be pulled from
            }

        } else if (weaponOut == 2) {
            for (int i = 0; i < Shotgunshells; i++) {
                Vector3 ShotgunAim = Camera.main.transform.forward;
                Quaternion AimSpread = Quaternion.LookRotation(ShotgunAim);
                Quaternion SpreadGenerator = Random.rotation;
                AimSpread = Quaternion.RotateTowards(AimSpread, SpreadGenerator, Random.Range(0.0f, ShotgunSpread));
                RaycastHit hit3;
                if (Physics.Raycast(Camera.main.transform.position, AimSpread * Vector3.forward, out hit3, Mathf.Infinity)) {

                    if (hit3.transform.tag == "Player") {
                        Shotgun_shot.Play();
                        ShotGunParticle.Play();
                        float distance = Vector3.Distance(transform.position, hit3.transform.position);
                        if (distance >= 100) {
                            distance = 99;
                        }
                        HitMarker.SetActive(true);
                        ShotgunDmg = ShotgunDmg * 1 - (distance / 100);
                        int damageS = (int)ShotgunDmg;
                        CmdDamageDealer(hit3.transform.gameObject, damageS, currentPlayer);
                        spawnhole = false;
                    }
                }
            }
        } else if (weaponOut == 3 && Physics.Raycast(Camera.main.transform.position, childRoot.transform.forward, out hit2)) {
            if (hit2.transform.tag == "Player") {
                Sniper_shot.Play();
                SniperParticle.Play();
                float distance = Vector3.Distance(transform.position, hit2.transform.position);
                if (distance >= 500) {
                    distance = 499;
                }
                HitMarker.SetActive(true);
                sniperDmg = sniperDmg * 1 - (distance / 500);
                int damageSn = (int)sniperDmg;
                CmdDamageDealer(hit2.transform.gameObject, damageSn, currentPlayer);
                spawnhole = false;

                //HitMarkersound here;
                //Enemy enemyhealth = hit2.collider.gameObject.GetComponent<Enemy>();
                //enemyhealth.TakeDamage(10);  // always sending consistent damage, this will have to be pulled from
            }
        } else {
            Debug.Log("Missed player");
        }
        if (weaponOut == 1) {
            fire.Play();
            ShotParticle.Play();
            //ShotFlash.Play();
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, childRoot.transform.forward);
            if (Physics.Raycast(ray, out hit, 100f) && spawnhole) {
                Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Instantiate(bulletHole1, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            }
            spawnhole = true;

        } else if (weaponOut == 2) {
            Shotgun_shot.Play();
            RaycastHit hit4;
            for (int i = 0; i < Shotgunshells; i++) {
                Vector3 ShotgunAim = Camera.main.transform.forward;
                Quaternion AimSpread = Quaternion.LookRotation(ShotgunAim);
                Quaternion SpreadGenerator = Random.rotation;
                AimSpread = Quaternion.RotateTowards(AimSpread, SpreadGenerator, Random.Range(0.0f, ShotgunSpread));
                if (Physics.Raycast(Camera.main.transform.position, AimSpread * Vector3.forward, out hit4, Mathf.Infinity)) {
                    ShotGunParticle.Play();
                    Instantiate(bulletHole, hit4.point, Quaternion.FromToRotation(Vector3.up, hit4.normal));
                    Instantiate(bulletHoleSG1, hit4.point, Quaternion.FromToRotation(Vector3.up, hit4.normal));
                }
                spawnhole = true;
            }
        } else if (weaponOut == 3) {
            Sniper_shot.Play();
            SniperParticle.Play();
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, childRoot.transform.forward);
            if (Physics.Raycast(ray, out hit, 100f) && spawnhole) {
                Instantiate(bulletHoleS, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Instantiate(bulletHoleS1, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            }
            spawnhole = true;

        } else {
            Debug.Log("Not Shotting or not Hit player");
        }
    }
    /*
    [Command]
    void CmdBulletHoleRifle(RaycastHit hit) {
        GameObject BulletHole = (GameObject)Instantiate(bulletHole, transform.root.transform.position + new Vector3(0, 1, 0) + transform.forward, Quaternion.identity) as GameObject;//***
        NetworkServer.Spawn(BulletHole);
    }
    [Command]
    void CmdBulletHoleShotgun(RaycastHit hit) {

    }
    [Command]
    void CmdBulletHoleSniper(RaycastHit hit) {

    }
    */
    /*
    void CheckWeaponNumber(int playerNum) {
       // if (!isLocalPlayer) {//????
       //     return;
       // }
        if (currentWeaponPlayer == playerNum) {
            loseWeapon();//weapon information??
        }
    }
    */


    [Command]
    void CmdDamageDealer(GameObject hit, int damage, int playerNumber) {
        int[] tempSend = new int[2];
        tempSend[0] = damage;
        tempSend[1] = playerNumber;
        hit.SendMessage("TakeDamage", tempSend);
    }

    IEnumerator hit() {
        yield return new WaitForSeconds(hitmarkertime);
        HitMarker.SetActive(false);
    }

    IEnumerator Reload() {
        //reload.Play();
        inReload = true;
        canShoot = false;

        //transform.Find("crystal").gameObject.GetComponent<Animation>().Play("Take 001");
        yield return new WaitForSeconds(RELOAD_TIME);
        currentWeaponAmmo = currentWeaponMaxAmmo;

        canShoot = true;
        inReload = false;

    }
    void CheckCanShoot() {
        if (inReload || !hasWeapon) {
            return;
        }
        if (currentWeaponAmmo == 0) {
            canShoot = false;
        }
        if (currentWeaponAmmo > 0) {
            canShoot = true;
        }
    }

    [Command]
    private void CmdDamIDied(GameObject[] listOFPlayers, int thisPlayer) {
        foreach (GameObject player in listOFPlayers) {
            if (player.GetComponent<weaponManager>().currentWeaponPlayer == thisPlayer) {
                player.SendMessage("NeedToDropWeapon");
            }
        }
    }

    public void DamIDied() {
        if (currentPlayer != currentWeaponPlayer) {
            dropWeapon();
        }
        GameObject[] output = new GameObject[plrMngr.Players.Count];
        for (int i = 0; i < plrMngr.Players.Count; i++) {
            output[i] = plrMngr.Players[i];
        }
        CmdDamIDied(output, currentPlayer);
    }
    public void NeedToDropWeapon() {
        if (!isServer) {
            return;
        }
        dropIt = true;
    }
    [Command]
    private void CmdKIDroppedIt(GameObject thePlayer) {
        dropIt = false;
        //hasDroppedOne = false;
        RpcSingleWeaponDrop(thePlayer);
    }
    [ClientRpc]
    void RpcSingleWeaponDrop(GameObject thePlayer) {
        thePlayer.GetComponent<weaponManager>().hasDroppedOne = false;
    }
    void lostWeapon() {
        loseWeapon();
        hasDroppedOne = true;
        CmdKIDroppedIt(this.gameObject);
    }
}

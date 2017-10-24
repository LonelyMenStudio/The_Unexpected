using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class weaponManager : NetworkBehaviour {

    private bool startedAuto = false;
    private bool picking = false;
    public GameObject shotty;
    public GameObject ak;
    public GameObject Sniper;
    public int weaponOut = 0;//1 rifle, 2 shotgun, 3 sniper, 4 crappy
    private int secondWeapon = 0;
    public bool hasWeapon = false;
    public int currentWeaponAmmo;
    public int currentWeaponMaxAmmo;
    private float AkDamage = 20;
    private float ShotgunDmg = 20;
    private float sniperDmg = 90;
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
    private PrepPhase gObject;
    public AudioSource fire;
    public AudioSource reload;
    public AudioSource HitSE;
    public AudioSource Sniper_shot;
    public AudioSource Shotgun_shot;
    public AudioSource empty;
    private float delayTime = 0.05f;
    private float counter = 0.0f;
    private bool canShoot = false;
    public GameObject bulletHole;
    public GameObject bulletHole1;
    public GameObject Hole;
    public GameObject bulletHoleS;
    public GameObject bulletHoleSG;
    public GameObject bulletHoleSG1;
    public GameObject bulletHoleS1;
    public ParticleSystem ShotParticle;
    public ParticleSystem ShotFlash;
    public ParticleSystem ShotGunParticle;
    public ParticleSystem SniperParticle;
    private GameObject Gunout1;
    private GameObject Gunout2;
    private GameObject Gunout3;
    public GameObject Gunout4;
    public GameObject Gunout5;
    public GameObject Gunout6;
    // public AudioSource[] sounds;
    public GameObject AmmoObject;
    private bool spawnhole = true;
    public int Shotgunshells = 6;
    public float ShotgunSpread = 10.0f;
    private bool inReload = false;
    private PlayerAssignGet pl;
    public int currentPlayer;
    public bool hasDroppedOne = false;
    public bool isSprinting = false;
    public int temp;

    // vvv- for balancing purpose too -vvv
    private int rifleAmmo = 45;
    private int shottyAmmo = 18;
    private int sniperAmmo = 3;
    private float rifleFireRate = 0.08f;
    private float shottyFireRate = 0.65f;
    private float sniperFireRate = 1.5f;
    // ^^^- end of balancing variables -^^^

    public bool isCrapGun = false;
    private int crappyAmmo = 30;
    private float crappyFireRate = 0.08f;
    private float crappyDamage = 10;
    public GameObject crappyGunObject;
    public Material crappyMat;
    public Material standardMat;
    public GameObject weaponMatChange;

    [SyncVar]
    public int currentWeaponPlayer = 0;
    [SyncVar]
    public bool dropIt = false;
    [SyncVar]
    private bool crappyTexture = false;

    private bool inWeaponSelect = true;
    private bool selectionDone = false;
    private int weaponOnEnd = 0;
    private GameObject[] weaponRespawnLocation;
    private GameObject manager;
    private PlayerAssign wrl;
    private PlayerManagerSelf playerManage;
    private bool checkingPrep = true;
    private bool actionOnce = true;
    public GameObject Variables;
    private VariablesScript ManagerGet;
    private PlayerManager plrMngr;
    private IKControl weaponhold;
    private Playeranimations Aimming;
    private bool TakeAim2 = true;
    private int weaponRespawnAmount = 0;
    private bool analyticsgotwep = true;

    //=======


    //>>>>>>> c3943934d6b06f638b2c283b56224c49b4642929
    //Animator animatorz;

    private const int distancePickup = 5;
    private const float RELOAD_TIME = 3.0f;
    private const float hitmarkertime = 1.0f;

    void Awake() {
        Variables = GameObject.FindWithTag("Start");
    }

    // Use this for initialization
    void Start() {
        Aimming = GetComponent<Playeranimations>();
        weaponhold = GetComponent<IKControl>();
        ManagerGet = Variables.GetComponent<VariablesScript>();
        manager = ManagerGet.variables;
        gObject = manager.GetComponent<PrepPhase>();
        HitMarker = gObject.HitMark;
        Hitmark = HitMarker.GetComponent<Image>();
        HitMarker.SetActive(false);
        AmmoObject = GameObject.FindWithTag("Ammo");
        ammoText = AmmoObject.GetComponent<Text>();
        wrl = manager.GetComponent<PlayerAssign>();
        playerManage = this.gameObject.GetComponent<PlayerManagerSelf>();
        weaponRespawnAmount = wrl.weaponRespawnPoints.Length - 1;
        weaponRespawnLocation = new GameObject[wrl.weaponRespawnPoints.Length];
        for (int i = 0; i < wrl.weaponRespawnPoints.Length; i++) {
            weaponRespawnLocation[i] = wrl.weaponRespawnPoints[i];
        }
        if (isLocalPlayer) {
            body.layer = 2;
        }
        childRoot = transform.Find("FirstPersonCharacter").gameObject;
        pl = this.gameObject.GetComponent<PlayerAssignGet>();
        plrMngr = manager.GetComponent<PlayerManager>();

        if (!isLocalPlayer) {
            return;
        }
        Gunout1 = gObject.HUGgun1;
        Gunout2 = gObject.HUGgun2;
        Gunout3 = gObject.HUGgun3;
        Gunout1.SetActive(false);
        Gunout2.SetActive(false);
        Gunout3.SetActive(false);
        Gunout4 = gObject.HUGgun4;
        Gunout5 = gObject.HUGgun5;
        Gunout6 = gObject.HUGgun6;
        Gunout4.SetActive(false);
        Gunout5.SetActive(false);
        Gunout6.SetActive(false);
    }


    // Update is called once per frame
    void Update() {
        if (crappyTexture) {
            weaponMatChange.gameObject.GetComponent<Renderer>().material = crappyMat;
        } else {
            weaponMatChange.gameObject.GetComponent<Renderer>().material = standardMat;
        }
        if (!isLocalPlayer) {
            return;
        }

        if (hasWeapon == false) {
            currentWeaponAmmo = 0;
            currentWeaponMaxAmmo = 0;
            CmdSetWeaponPlayer(0);
        }
        CheckCanShoot();
        AutoReload();
        if (dropIt == true && !hasDroppedOne) {
            lostWeapon();
        }
        if (checkingPrep && !gObject.inPrep) {
            inWeaponSelect = false;
            selectionDone = true;
            checkingPrep = false;
        }
        if (this.GetComponent<PauseMenu>().isPaused) {
            return;
        }
        //Weapon Select
        if (inWeaponSelect) {
            currentPlayer = pl.currentPlayerNo;
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0) && !hasWeapon) {  // need to investigate further control options
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, childRoot.transform.forward, out hit, distancePickup)) {
                    if (hit.transform.tag == "weaponPrep") {
                        weaponhold.ikActive = true;
                        hasWeapon = true;
                        if (hit.transform.gameObject.name.Contains("PrepRifle")) {
                            weaponOnEnd = 0;
                            //weaponOut = 1
                            childWeapon1.SetActive(false);//***
                            childWeapon2.SetActive(false);//***
                            childWeapon3.SetActive(false);//***
                            Gunout1.SetActive(true);
                            Gunout2.SetActive(false);
                            Gunout3.SetActive(false);
                            changeWeapon(1);
                            currentWeaponAmmo = rifleAmmo;
                            currentWeaponMaxAmmo = rifleAmmo;
                            delayTime = rifleFireRate;
                        } else if (hit.transform.gameObject.name.Contains("PrepShotty")) {
                            weaponOnEnd = 1;
                            childWeapon1.SetActive(false);//***
                            childWeapon2.SetActive(false);//***
                            childWeapon3.SetActive(false);//***
                            Gunout1.SetActive(false);
                            Gunout2.SetActive(true);
                            Gunout3.SetActive(false);
                            changeWeapon(2);
                            currentWeaponAmmo = shottyAmmo;
                            currentWeaponMaxAmmo = shottyAmmo;
                            delayTime = shottyFireRate;
                        } else if (hit.transform.gameObject.name.Contains("PrepSniper")) {
                            weaponOnEnd = 2;
                            childWeapon1.SetActive(false);//***
                            childWeapon2.SetActive(false);//***
                            childWeapon3.SetActive(false);//***
                            Gunout1.SetActive(false);
                            Gunout2.SetActive(false);
                            Gunout3.SetActive(true);
                            changeWeapon(3);
                            currentWeaponAmmo = sniperAmmo;
                            currentWeaponMaxAmmo = sniperAmmo;
                            delayTime = sniperFireRate;
                        }
                    }
                }
            }
            //flaggity flag
            if (hasWeapon) {
                canShoot = true;
            } else {
                canShoot = false;
            }
            counter += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.R) && hasWeapon && (currentWeaponAmmo < currentWeaponMaxAmmo)) {
                StartCoroutine(Reload());
            }
            if (Input.GetKey(KeyCode.Mouse0) && hasWeapon && canShoot && counter > delayTime && !isSprinting && TakeAim2) { // probs can be cut down to only 1 raycast
                if (!Aimming.Aim) {
                    Aimming.Change = true;
                    Aimming.Aim = true;
                    TakeAim2 = false;
                    StartCoroutine(TakeAim());

                } else {
                    Aimming.Aim = true;
                    if (currentWeaponAmmo > 0) {
                        shoot();
                    } else {
                        if (Input.GetKeyDown(KeyCode.Mouse0)) {
                            empty.Play();
                        }
                    }
                }
            }
            ammoText.text = currentWeaponAmmo + "/" + currentWeaponMaxAmmo;
            return;
        }
        if (selectionDone && actionOnce) {
            actionOnce = false;
            weaponFirstSpawn(weaponOnEnd);
        }

        if (Input.GetKey(KeyCode.Mouse0) && hasWeapon && canShoot && counter > delayTime && !isSprinting && TakeAim2) {
            if (!Aimming.Aim) {
                Aimming.Change = true;
                Aimming.Aim = true;
                TakeAim2 = false;
                StartCoroutine(TakeAim());

            } else {
                Aimming.Aim = true;
                shoot();

            }

        }

        counter += Time.deltaTime;//counter to ensure not infinite fire rate
        if (Input.GetKeyDown(KeyCode.R) && hasWeapon && (currentWeaponAmmo < currentWeaponMaxAmmo)) {
            StartCoroutine(Reload());
        }
        if ((Input.GetKeyDown(KeyCode.E) || (Input.GetKeyDown(KeyCode.Mouse0) && !hasWeapon)) && !picking) {
            StartCoroutine(pickupTime());
            PickupWeapon();

        }
        if (Input.GetKeyDown(KeyCode.F) && hasWeapon) {
            if (weaponOut == 1) {
                Gunout4.SetActive(true);
                Gunout5.SetActive(false);
                Gunout6.SetActive(false);
            } else if (weaponOut == 2) {
                Gunout4.SetActive(false);
                Gunout5.SetActive(true);
                Gunout6.SetActive(false);
            } else if (weaponOut == 3) {
                Gunout4.SetActive(false);
                Gunout5.SetActive(false);
                Gunout6.SetActive(true);
            }
            dropWeapon();
            weaponhold.ikActive = false;
            childWeapon1.SetActive(false);//***
            childWeapon2.SetActive(false);//***
            childWeapon3.SetActive(false);//***
            Gunout1.SetActive(false);
            Gunout2.SetActive(false);
            Gunout3.SetActive(false);

        }
        ammoText.text = currentWeaponAmmo + "/" + currentWeaponMaxAmmo;
        if (currentWeaponAmmo <= 0) {
            canShoot = false;
        }

    }

    IEnumerator pickupTime() {
        picking = true;
        yield return new WaitForSeconds(1.0f);
        picking = false;
    }

    void AutoReload() {
        if (currentWeaponAmmo == 0 && !startedAuto && hasWeapon && !inWeaponSelect) {
            startedAuto = true;
            StartCoroutine(Reload());
            this.GetComponent<Playeranimations>().ReloadAnim();
        }
    }

    void PickupWeapon() {
        if (weaponOut == 0 && hasWeapon) {
            switchWeapon();
        }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, childRoot.transform.forward, out hit, distancePickup)) {
            if (hit.transform.tag == "weapon") {
                if (analyticsgotwep == true) {
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "In Game", "Found First Gun");
                    analyticsgotwep = false;
                }
                weaponhold.ikActive = true;
                if (hit.transform.gameObject.name.Contains("alienrifle")) {
                    weaponMatChange.gameObject.GetComponent<Renderer>().material = standardMat;
                    CmdSetCrappyTexture(false);
                    replaceWeapon(hit);
                    changeWeapon(1);
                    isCrapGun = false;
                    childWeapon1.SetActive(false);//***
                    childWeapon2.SetActive(false);//***
                    childWeapon3.SetActive(false);//***
                    Gunout1.SetActive(true);
                    Gunout2.SetActive(false);
                    Gunout3.SetActive(false);

                }
                if (hit.transform.gameObject.name.Contains("Shotty")) {
                    replaceWeapon(hit);
                    changeWeapon(2);
                    isCrapGun = false;
                    childWeapon1.SetActive(false);//***
                    childWeapon2.SetActive(false);//***
                    childWeapon3.SetActive(false);//***
                    Gunout1.SetActive(false);
                    Gunout2.SetActive(true);
                    Gunout3.SetActive(false);
                }
                if (hit.transform.gameObject.name.Contains("AlienSniper")) {
                    replaceWeapon(hit);
                    changeWeapon(3);
                    isCrapGun = false;
                    childWeapon1.SetActive(false);//***
                    childWeapon2.SetActive(false);//***
                    childWeapon3.SetActive(false);//***
                    Gunout1.SetActive(false);
                    Gunout2.SetActive(false);
                    Gunout3.SetActive(true);
                }
                if (hit.transform.gameObject.name.Contains("crappyRifle")) {
                    weaponMatChange.gameObject.GetComponent<Renderer>().material = crappyMat;
                    CmdSetCrappyTexture(true);
                    replaceWeapon(hit);
                    changeWeapon(1);
                    isCrapGun = true;
                    childWeapon1.SetActive(false);//***
                    childWeapon2.SetActive(false);//***
                    childWeapon3.SetActive(false);//***
                    Gunout1.SetActive(true);
                    Gunout2.SetActive(false);
                    Gunout3.SetActive(false);
                }
            }
        }
    }

    [Command]
    void CmdSetWeaponPlayer(int setTo) {
        currentWeaponPlayer = setTo;
    }

    [Command]
    void CmdSetCrappyTexture(bool i) {
        crappyTexture = i;
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
            childMelee.SetActive(true);//***
            childWeapon1.SetActive(false);//***
            childWeapon2.SetActive(false);//***
            childWeapon3.SetActive(false);//***
        }
        if (weapon == 1) {
            weaponOut = 1;
            delayTime = rifleFireRate;
            childMelee.SetActive(false);//***
            childWeapon1.SetActive(true);//***
            childWeapon2.SetActive(false);//***
            childWeapon3.SetActive(false);//***
        } else if (weapon == 2) {
            weaponOut = 2;
            delayTime = shottyFireRate;
            childMelee.SetActive(false);//***
            childWeapon1.SetActive(false);//***
            childWeapon2.SetActive(true);//***
            childWeapon3.SetActive(false);//***
        } else if (weapon == 3) {
            weaponOut = 3;
            delayTime = sniperFireRate;
            childMelee.SetActive(false);//***
            childWeapon1.SetActive(false);//***
            childWeapon2.SetActive(false);//***
            childWeapon3.SetActive(true);//***
        }
    }

    private void RespawnAK() {
        CmdRespawnAK();
        CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
    }

    private void spawnAK() {
        CmdSpawnAK();
    }

    private void destoryWeapon(GameObject destoryThis) {
        CmdDestroyHit(destoryThis);
    }

    private void spawnPistol() {
        CmdSpawnPistol();
    }

    private void RespawnPistol() {
        CmdRespawnPistol();
        CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
    }

    private void spawnSniper() {
        CmdSpawnSniper();
    }

    private void RespawnSniper() {
        CmdRespawnSniper();
        CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
    }

    [Command]
    void CmdRespawnAK() {
        int temp2 = temp;
        while (temp2 == temp) {
            temp = Random.Range(0, weaponRespawnAmount);
        }
        GameObject weaponDropper = (GameObject)Instantiate(ak, weaponRespawnLocation[temp].transform.position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(weaponDropper);
        weaponDropperTemp = weaponDropper;
    }

    [Command]
    void CmdRespawnPistol() {
        int temp2 = temp;
        while (temp2 == temp) {
            temp = Random.Range(0, weaponRespawnAmount);
        }
        GameObject weaponDropper = (GameObject)Instantiate(shotty, weaponRespawnLocation[temp].transform.position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(weaponDropper);
        weaponDropperTemp = weaponDropper;
    }

    [Command]
    void CmdSpawnAK() {
        GameObject weaponDropper = (GameObject)Instantiate(ak, transform.root.transform.position + new Vector3(0, 1, 0) + transform.forward, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(weaponDropper);
        weaponDropperTemp = weaponDropper;
    }

    [Command]
    void CmdSpawnCrappy() {
        GameObject weaponDropper = (GameObject)Instantiate(crappyGunObject, transform.root.transform.position + new Vector3(0, 1, 0) + transform.forward, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(weaponDropper);
        weaponDropperTemp = weaponDropper;
    }

    [Command]
    void CmdSpawnPistol() {
        GameObject weaponDropper = (GameObject)Instantiate(shotty, transform.root.transform.position + new Vector3(0, 1, 0) + transform.forward * 2, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(weaponDropper);
        weaponDropperTemp = weaponDropper;
    }

    [Command]
    void CmdRespawnSniper() {
        int temp2 = temp;
        while (temp2 == temp) {
            temp = Random.Range(0, weaponRespawnAmount);
        }
        GameObject weaponDropper = (GameObject)Instantiate(Sniper, weaponRespawnLocation[temp].transform.position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(weaponDropper);
        weaponDropperTemp = weaponDropper;
    }

    [Command]
    void CmdSpawnSniper() {
        GameObject weaponDropper = (GameObject)Instantiate(Sniper, transform.root.transform.position + new Vector3(0, 1, 0) + transform.forward * 2, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(weaponDropper);
        weaponDropperTemp = weaponDropper;
    }

    [Command]
    void CmdDestroyHit(GameObject objectToDestory) {
        weaponSettings clearWeapon = objectToDestory.GetComponent<weaponSettings>();
        NetworkServer.Destroy(objectToDestory.transform.gameObject);
    }

    void weaponFirstSpawn(int weaponToSpawn) {
        hasWeapon = false;
        canShoot = false;
        Gunout1.SetActive(false);
        Gunout2.SetActive(false);
        Gunout3.SetActive(false);
        if (!isServer) {
            CmdSwitchWeapon(0);
        } else {
            RpcSwitchWeapon(0);
        }
        if (weaponToSpawn == 0) {
            CmdRespawnAK();
            Gunout4.SetActive(true);
            Gunout5.SetActive(false);
            Gunout6.SetActive(false);
            CmdWeaponAmmoDrop(weaponDropperTemp, rifleAmmo, rifleAmmo, currentPlayer);
        } else if (weaponToSpawn == 1) {
            CmdRespawnPistol();
            Gunout4.SetActive(false);
            Gunout5.SetActive(true);
            Gunout6.SetActive(false);
            CmdWeaponAmmoDrop(weaponDropperTemp, shottyAmmo, shottyAmmo, currentPlayer);
        } else if (weaponToSpawn == 2) {
            CmdRespawnSniper();
            Gunout4.SetActive(false);
            Gunout5.SetActive(false);
            Gunout6.SetActive(true);
            CmdWeaponAmmoDrop(weaponDropperTemp, sniperAmmo, sniperAmmo, currentPlayer);
        } else {
            Debug.Log("problem spawning the weapon");
        }

    }

    void loseWeapon() {
        if (!isLocalPlayer) {
            return;
        }
        hasWeapon = false;
        canShoot = false;
        Gunout1.SetActive(false);
        Gunout2.SetActive(false);
        Gunout3.SetActive(false);
        if (!isServer) {
            CmdSwitchWeapon(0);
        } else {
            RpcSwitchWeapon(0);
        }
        if (weaponOut == 1) {
            RespawnAK();
            Gunout4.SetActive(true);
            Gunout5.SetActive(false);
            Gunout6.SetActive(false);
        }
        if (weaponOut == 2) {
            RespawnPistol();
            Gunout4.SetActive(false);
            Gunout5.SetActive(true);
            Gunout6.SetActive(false);
        }
        if (weaponOut == 3) {

            RespawnSniper();
            Gunout4.SetActive(false);
            Gunout5.SetActive(false);
            Gunout6.SetActive(true);
        }
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
            Gunout4.SetActive(true);
            Gunout5.SetActive(false);
            Gunout6.SetActive(false);
            if (!isCrapGun) {
                spawnAK();
            } else {
                CmdSpawnCrappy();
            }
            CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
        } else if (weaponOut == 2) {
            Gunout4.SetActive(false);
            Gunout5.SetActive(true);
            Gunout6.SetActive(false);
            spawnPistol();
            CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
        } else if (weaponOut == 3) {
            Gunout4.SetActive(false);
            Gunout5.SetActive(false);
            Gunout6.SetActive(true);
            spawnSniper();
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
                if (!isCrapGun) {
                    spawnAK();
                } else {
                    CmdSpawnCrappy();
                }
                CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo, currentWeaponPlayer);
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
        CmdSetWeaponPlayer(currentWeaponSettingsGet.playerNo);
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
                if (!checkingPrep) {
                    if (!isCrapGun) {
                        AkDamage = AkDamage * 1 - (distance / 300);
                        int damageAK = (int)AkDamage;
                        CmdDamageDealer(hit2.transform.gameObject, damageAK, currentPlayer);
                    } else {
                        crappyDamage = crappyDamage * 1 - (distance / 300);
                        int damageCrappy = (int)crappyDamage;
                        CmdDamageDealer(hit2.transform.gameObject, damageCrappy, currentPlayer);
                    }
                }
                spawnhole = false;
                HitMarker.SetActive(true);
                StartCoroutine(hit());
            }
            if (hit2.transform.tag == "Crystal") {
                prepareDamageCrystal(hit2.transform.gameObject, AkDamage);
            }
            if (hit2.transform.tag == "Bot") {
                BotMovement call = hit2.transform.gameObject.GetComponent<BotMovement>();
                call.TakeDamage(AkDamage);
                spawnhole = false;
                HitMarker.SetActive(true);
                StartCoroutine(hit());
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
                        if (!isServer) {
                            CmdPlayShotgunAudio();
                        } else {
                            RpcPlayShotgunAudio();
                        }
                        ShotGunParticle.Play();
                        float distance = Vector3.Distance(transform.position, hit3.transform.position);
                        if (distance >= 100) {
                            distance = 99;
                        }
                        HitMarker.SetActive(true);
                        StartCoroutine(hit());
                        if (!checkingPrep) {
                            ShotgunDmg = ShotgunDmg * 1 - (distance / 100);
                            int damageS = (int)ShotgunDmg;
                            HitSE.Play();
                            CmdDamageDealer(hit3.transform.gameObject, damageS, currentPlayer);
                        }
                        spawnhole = false;
                    }
                    if (hit3.transform.tag == "Crystal") {
                        prepareDamageCrystal(hit3.transform.gameObject, ShotgunDmg);
                    }
                    if (hit3.transform.tag == "Bot") {
                        BotMovement call = hit3.transform.gameObject.GetComponent<BotMovement>();
                        call.TakeDamage(ShotgunDmg);
                        spawnhole = false;
                        HitMarker.SetActive(true);
                        StartCoroutine(hit());
                    }
                }
            }
        } else if (weaponOut == 3 && Physics.Raycast(Camera.main.transform.position, childRoot.transform.forward, out hit2)) {
            if (hit2.transform.tag == "Player") {
                if (!isServer) {
                    CmdPlaySniperAudio();
                } else {
                    RpcPlaySniperAudio();
                }
                SniperParticle.Play();
                float distance = Vector3.Distance(transform.position, hit2.transform.position);
                if (distance >= 500) {
                    distance = 499;
                }
                HitMarker.SetActive(true);
                StartCoroutine(hit());
                if (!checkingPrep) {
                    sniperDmg = sniperDmg * 1 - (distance / 500);
                    int damageSn = (int)sniperDmg;
                    HitSE.Play();
                    CmdDamageDealer(hit2.transform.gameObject, damageSn, currentPlayer);
                }
                spawnhole = false;
            }
            if (hit2.transform.tag == "Crystal") {
                prepareDamageCrystal(hit2.transform.gameObject, sniperDmg);
            }
            if (hit2.transform.tag == "Bot") {
                BotMovement call = hit2.transform.gameObject.GetComponent<BotMovement>();
                call.TakeDamage(sniperDmg);
                spawnhole = false;
                HitMarker.SetActive(true);
                StartCoroutine(hit());
            }
        } else {
            Debug.Log("Missed player");
        }
        if (weaponOut == 1) {
            CmdPlayRifleEffect();
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, childRoot.transform.forward);
            if (Physics.Raycast(ray, out hit, 100f) && spawnhole) {
                CmdBulletHoleRifle(hit.point, hit.normal);
            }
            spawnhole = true;
        } else if (weaponOut == 2) {
            if (!isServer) {
                CmdPlayShotgunAudio();
            } else {
                RpcPlayShotgunAudio();
            }
            RaycastHit hit4;
            for (int i = 0; i < Shotgunshells; i++) {
                Vector3 ShotgunAim = Camera.main.transform.forward;
                Quaternion AimSpread = Quaternion.LookRotation(ShotgunAim);
                Quaternion SpreadGenerator = Random.rotation;
                AimSpread = Quaternion.RotateTowards(AimSpread, SpreadGenerator, Random.Range(0.0f, ShotgunSpread));
                if (Physics.Raycast(Camera.main.transform.position, AimSpread * Vector3.forward, out hit4, Mathf.Infinity)) {
                    ShotGunParticle.Play();
                    CmdBulletHoleShotgun(hit4.point, hit4.normal);
                }
                spawnhole = true;
            }
        } else if (weaponOut == 3) {
            if (!isServer) {
                CmdPlaySniperAudio();
            } else {
                RpcPlaySniperAudio();
            }
            SniperParticle.Play();
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, childRoot.transform.forward);
            if (Physics.Raycast(ray, out hit, 100f) && spawnhole) {
                CmdBulletHoleSniper(hit.point, hit.normal);
            }
            spawnhole = true;
        } else {
            Debug.Log("Not Shotting or not Hit player");
        }
    }

    [Command]
    void CmdDamageCrystal(GameObject hit, int[] message) {
        hit.SendMessage("DamageCrystal", message);
    }

    void prepareDamageCrystal(GameObject hit, float damage) {
        int[] prepare = new int[2];
        prepare[0] = (int)damage;
        prepare[1] = currentPlayer;
        CmdDamageCrystal(hit, prepare);
    }

    [Command]
    void CmdBulletHoleRifle(Vector3 hit, Vector3 normal) {
        GameObject BulletHole = (GameObject)Instantiate(bulletHole, hit, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject;//***
        NetworkServer.Spawn(BulletHole);
        BulletHole = (GameObject)Instantiate(bulletHole1, hit, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject;//***
        NetworkServer.Spawn(BulletHole);
    }

    [Command]
    void CmdBulletHoleShotgun(Vector3 point, Vector3 normal) {
        GameObject BulletHole = (GameObject)Instantiate(bulletHoleSG, point, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject;//***
        NetworkServer.Spawn(BulletHole);
        BulletHole = (GameObject)Instantiate(bulletHoleSG1, point, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject;//***
        NetworkServer.Spawn(BulletHole);
    }

    [Command]
    void CmdBulletHoleSniper(Vector3 point, Vector3 normal) {
        GameObject BulletHole = (GameObject)Instantiate(bulletHoleS, point, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject;//***
        NetworkServer.Spawn(BulletHole);
        BulletHole = (GameObject)Instantiate(bulletHoleS1, point, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject;//***
        NetworkServer.Spawn(BulletHole);
    }

    [Command]
    void CmdPlayRifleEffect() {
        RpcPlayRifleEffect();
    }

    [ClientRpc]
    void RpcPlayRifleEffect() {
        LclPlayRifleEffect();
    }

    void LclPlayRifleEffect() {
        fire.Play();
        ShotParticle.Play();
    }

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

    IEnumerator TakeAim() {

        yield return new WaitForSeconds(0.5f);
        TakeAim2 = true;
        //canShoot = true;
        // shoot();
        //Aimming.TakeAim();
    }

    IEnumerator Reload() {
        inReload = true;
        canShoot = false;
        if (!isServer) {
            CmdPlayReloadAudio();
        } else {
            RpcPlayReloadAudio();
        }
        yield return new WaitForSeconds(RELOAD_TIME);
        currentWeaponAmmo = currentWeaponMaxAmmo;
        canShoot = true;
        inReload = false;
        startedAuto = false;
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

    [Command]
    void CmdPlaySniperAudio() {
        RpcPlaySniperAudio();
    }

    [ClientRpc]
    void RpcPlaySniperAudio() {
        Sniper_shot.Play();
    }

    [Command]
    void CmdPlayShotgunAudio() {
        RpcPlayShotgunAudio();
    }

    [ClientRpc]
    void RpcPlayShotgunAudio() {
        Shotgun_shot.Play();
    }

    [Command]
    void CmdPlayReloadAudio() {
        RpcPlayReloadAudio();
    }

    [ClientRpc]
    void RpcPlayReloadAudio() {
        reload.Play();
    }
}

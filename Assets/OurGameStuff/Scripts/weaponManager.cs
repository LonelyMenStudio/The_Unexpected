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
    
    //Animator animatorz;

    private const float RELOAD_TIME = 2.0f;

    void Awake() {
        sounds = GetComponents<AudioSource>();
        fire = sounds[1];
       // reload = sounds[0];
        //animatorz = GetComponent<Animator>();
        prepHud = GameObject.Find("Manager");
        gObject = prepHud.GetComponent<PrepPhase>();
        ammoDisplay = gObject.ammoObject;
        ammoText = ammoDisplay.GetComponent<Text>();

    }

    // Use this for initialization
    void Start() {
        if (isLocalPlayer) {
            body.layer = 2;
        }
        childRoot = transform.Find("FirstPersonCharacter").gameObject;
    }

    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer) {
            return;
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
        if (Input.GetKeyDown(KeyCode.F) && hasWeapon) {
            switchWeapon();
        }
        //ammoText.text = currentWeaponAmmo + "/" +currentWeaponMaxAmmo;
        if(currentWeaponAmmo <= 0) {
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
                if (hit.transform.gameObject.name.Contains("crystal")) { //***
                    replaceWeapon(hit);
                    //ChangeWeapon(1);
                    if (!isServer) {
                        CmdSwitchWeapon(1);
                    } else {
                        RpcSwitchWeapon(1);
                    }
                }
                if (hit.transform.gameObject.name.Contains("ak")) {//***
                    replaceWeapon(hit);
                    //ChangeWeapon(2);
                    if (!isServer) {
                        CmdSwitchWeapon(2);
                    } else {
                        RpcSwitchWeapon(2);
                    }
                }

            }
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

    void replaceWeapon(RaycastHit hit) {
        if (!hasWeapon) {
            weaponAmmoPick(hit);
            childMelee.SetActive(false);
            hasWeapon = true;
            canShoot = true;
            return;
        } else {
            if (weaponOut == 1) {
                CmdSpawnAK();
                CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo);
            } else if (weaponOut == 2) {
                CmdSpawnPistol();
                CmdWeaponAmmoDrop(weaponDropperTemp, currentWeaponAmmo, currentWeaponMaxAmmo);
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
        CmdDestroyHit(weaponPicker);
    }

    [Command]
    void CmdWeaponAmmoDrop(GameObject weaponDropper, int inputC, int inputM) {
        weaponSettings currentWeaponSettings = weaponDropperTemp.GetComponent<weaponSettings>();
        currentWeaponSettings.currentAmmo = inputC;
        currentWeaponSettings.maxAmmo = inputM;
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

    
}

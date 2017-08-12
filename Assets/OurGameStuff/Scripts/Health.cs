using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
    //public GameObject Textz;

    public GameObject respawn;
    private const int maxHealth = 300;
    public Image Healthbar;
    public Image PlayerHud;
    Text text;
    public float fillAmount;
    public GameObject barImage;
    private GameObject prepHud;
    private GameObject Manager;
    private PrepPhase gObject;//DUPE
    private PrepPhase inPrep;//DUPE
    public string playerName;
    public GameObject manager;
    private PrepPhase ph;//DUPE
    private PlayerAssignGet playerNumber;
    private PlayerManager deathMessage;
    private GameObject HudImage;
    public AudioSource GetHit;
    public GameObject[] respawnLocations;
    public GameObject Variables;
    private VariablesScript ManagerGet;
    private NetworkXYZSync teleporter;


    [SyncVar(hook = "OnChangeHealth")]
    public int Healthz = maxHealth;

    public int healthL;

    void Awake() {
        Variables = GameObject.FindWithTag("Start");


    }
    // Use this for initialization
    void Start() {

        ManagerGet = Variables.GetComponent<VariablesScript>();
        manager = ManagerGet.variables;
        //playerID = GetComponent<PrepPhase>().playerIDs;
        ph = manager.GetComponent<PrepPhase>();
        ph.Players.Add(this.gameObject);
        //getRespawns = manager.GetComponent<PlayerAssign>();
        respawnLocations = new GameObject[ph.spawn.Length];
        for(int i=0; i < ph.spawn.Length; i++) {
            respawnLocations[i] = ph.spawn[i];
        }
        //To Find the Health Bar
        barImage = ph.healthObject;
        Healthbar = barImage.GetComponent<Image>();
        //To Find the player HUD
        HudImage = ph.PlayerHUD;
        PlayerHud = HudImage.GetComponent<Image>();
        //To Find Player number and Send massage to PlayerManager
        inPrep = manager.GetComponent<PrepPhase>();//dupe
        playerNumber = this.gameObject.GetComponent<PlayerAssignGet>();//should work
        deathMessage = manager.GetComponent<PlayerManager>();
        teleporter = this.gameObject.GetComponent<NetworkXYZSync>();
        
    }

    public void TakeDamage(int[] damageInfo) {
        if (!isServer) {
            return;
        }
        int amount = damageInfo[0];
        int damageFrom = damageInfo[1];
        Healthz -= amount;
        /* bool getDamage = true;
         if (getDamage) {
             Color Opaque = new Color(1, 1, 1, 1);
             PlayerHud.color = Color.Lerp(PlayerHud.color, Opaque, 20 * Time.deltaTime);
             if (PlayerHud.color.a >= 0.8) {
                 getDamage = false;
             }
         }
         if (!getDamage) {
             Color Transparent = new Color(1, 1, 1, 0);
             PlayerHud.color = Color.Lerp(PlayerHud.color, Transparent, 20 * Time.deltaTime);
         }*/
        StartCoroutine(Flash());
        GetHit.Play();

        if (Healthz <= 0) {
            Healthz = 0;
            sendKill(damageFrom);//All good???

            //why respawn half here??
            //gameObject.transform.position = respawn.transform.position;
            //SEND MESSAGE BACK
        }
    }


    [Command]
    void CmdRespawn() {
        Healthz = maxHealth;
        
        playerNumber.deaths++;
    }

    void sendKill(int killerNumber) {
        for (int i = 0; i <= deathMessage.Players.Count; i++) {
            PlayerAssignGet checkPlayer = deathMessage.Players[i].GetComponent<PlayerAssignGet>();
            if (checkPlayer.currentPlayerNo == killerNumber) {
                checkPlayer.CmdIncreaseKill();
                return;
            }
        }
        Debug.Log("couldnt give kill");
    }
    // Update is called once per frame
    void Update() {

        healthL = Healthz;
        if(healthL <= 0) {
            teleporter.Teleport(respawnLocations[Random.Range(0, respawnLocations.Length)].transform.position);
        }
       //player dying animation player wait for done then reset to give feedback
       if(Healthz <= 0) {
            
            
            CmdPlayerDied(playerNumber.currentPlayerNo);
            CmdRespawn();
            teleporter.Teleport(respawnLocations[Random.Range(0, respawnLocations.Length)].transform.position);
            if (isLocalPlayer) {
                
                //will act for everyone as all versions of player will die

                //Respawn();
            }
        }
       //Reset back into game
       

        if (Input.GetKeyDown("o") && isLocalPlayer) {
            CmdTestDamage();
        }
    }
    [Command]
    void CmdTestDamage() {
        Healthz = Healthz - 20;
    }

    [Command]
    void CmdPlayerDied(int playerNum) {
        deathMessage.deathMessenger(playerNum);
    }

    void OnChangeHealth(int health) {
        if (isLocalPlayer) {
            Healthbar.fillAmount = Map(health, 300, 0, 0, 1);
        }
        /*if(isLocalPlayer){
            text.text = health + "";
        }*/

    }

    private float Map(float health, float max, float min, float fillMin, float fillMax) {
        return (health - min) * (fillMax - fillMin) / (max - min) + fillMin;
    }


    IEnumerator Flash() {
            
            PlayerHud.color = Color.Lerp(PlayerHud.color,  Color.red, 30 * Time.deltaTime);
            yield return new WaitForSeconds(0.8F);
            PlayerHud.color = new Color(255, 255, 255, 255);
    }

}

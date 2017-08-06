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
    private PrepPhase gObject;
    private PrepPhase inPrep;
    public string playerName;
    //public int playerID;
    public GameObject manager;
    private PrepPhase ph;
    private PlayerAssign playerNumber;
    private PlayerManager deathMessage;
    private GameObject HudImage;
    public AudioSource GetHit;


    public GameObject Variables;
    private VariablesScript ManagerGet;

    [SyncVar(hook = "OnChangeHealth")]
    public int Healthz = maxHealth;


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
        //gameObject.name = "Player" + playerID;

        //barImage = GameObject.Find("HealthBar");
        //prepHud = GameObject.Find("PrepPhaseManager");
        //gObject = prepHud.GetComponent<PrepPhase>();
        barImage = ph.healthObject;
        Healthbar = barImage.GetComponent<Image>();
        HudImage = ph.PlayerHUD;
        PlayerHud = HudImage.GetComponent<Image>();
        inPrep = manager.GetComponent<PrepPhase>();
        playerNumber = this.gameObject.GetComponent<PlayerAssign>();//should work
        deathMessage = manager.GetComponent<PlayerManager>();
        
    }

    public void TakeDamage(int amount) {
        if (!isServer) {
            return;
        }
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
            gameObject.transform.position = respawn.transform.position;
        }


    }
    // Update is called once per frame
    void Update() {
        /*

       //player dying animation player wait for done then reset to give feedback
       if(Healthz == 0) {
           deathMessage.CmdPlayerDied(playerNumber.playerNo);
       }
       //Reset back into game
       */

        if (Input.GetKeyDown("o")) {
            TakeDamage(10);
        }
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
            yield return new WaitForSeconds(1);
            PlayerHud.color = new Color(255, 255, 255, 255);
    }

}

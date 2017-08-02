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

    [SyncVar(hook = "OnChangeHealth")]
    public int Healthz = maxHealth;

    // Use this for initialization
    void Start() {
        //playerID = GetComponent<PrepPhase>().playerIDs;
        manager = GameObject.FindWithTag("Start");
        ph = manager.GetComponent<PrepPhase>();
        ph.Players.Add(this.gameObject);
        //gameObject.name = "Player" + playerID;

        //barImage = GameObject.Find("HealthBar");
        prepHud = GameObject.Find("PrepPhaseManager");
        //gObject = prepHud.GetComponent<PrepPhase>();
        //barImage = gObject.healthObject;
        //Healthbar = barImage.GetComponent<Image>();
        Manager = GameObject.Find("Manager");
        inPrep = Manager.GetComponent<PrepPhase>();

    }

    public void TakeDamage(int amount) {
        if (!isServer) {
            return;
        }
        Healthz -= amount;
        if (Healthz <= 0) {
            Healthz = 0;
            gameObject.transform.position = respawn.transform.position;
        }


    }
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("o")) {
            //TakeDamage(10);
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

}

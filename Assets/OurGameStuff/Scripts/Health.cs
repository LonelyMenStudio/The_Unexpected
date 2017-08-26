using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public GameObject respawn;
    private const int maxHealth = 300;
    public Image Healthbar;
    public Image PlayerHud;
    public Image DamageScreen;
    Text text;
    public float fillAmount;
    public GameObject barImage;
    private GameObject prepHud;
    private GameObject Manager;
    private PrepPhase prepPhase;
    public string playerName;
    public GameObject manager;
    private PlayerAssignGet playerNumber;
    private PlayerManager deathMessage;
    private GameObject HudImage;
    public AudioSource GetHit;
    public GameObject[] respawnLocations;
    public GameObject Variables;
    private VariablesScript ManagerGet;
    private NetworkXYZSync teleporter;
    private bool inRespawn = false;
    public bool death = false;
    private GameObject HitMarker;
    //private Color red;
    //private Color reset;

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
        prepPhase = manager.GetComponent<PrepPhase>();
        prepPhase.Players.Add(this.gameObject);
        respawnLocations = new GameObject[prepPhase.spawn.Length];
        for (int i = 0; i < prepPhase.spawn.Length; i++) {
            respawnLocations[i] = prepPhase.spawn[i];
        }
        barImage = prepPhase.healthObject;
        Healthbar = barImage.GetComponent<Image>();
        HudImage = prepPhase.PlayerHUD;
        HitMarker = prepPhase.HitScreen;
        HitMarker.SetActive(false);
        DamageScreen = HitMarker.GetComponent<Image>();
        PlayerHud = HudImage.GetComponent<Image>();
        playerNumber = this.gameObject.GetComponent<PlayerAssignGet>();
        deathMessage = manager.GetComponent<PlayerManager>();
        teleporter = this.gameObject.GetComponent<NetworkXYZSync>();
        healthL = maxHealth;
        //reset = new Color(50, 80, 150, 255);
        //PlayerHud.color = reset;
    }

    public void TakeDamage(int[] damageInfo) {
        if (!isServer) {
            return;
        }
        int amount = damageInfo[0];
        int damageFrom = damageInfo[1];
        Healthz -= amount;
        if (Healthz <= 0) {
            Healthz = 0;
            sendKill(damageFrom);
        }
    }


    [Command]
    void CmdRespawn(GameObject toPlayer) {
        Healthz = maxHealth;
        playerNumber.deaths++;
        RpcCanResapwnAgain(toPlayer);
        //inRespawn = false;
    }
    [ClientRpc]
    void RpcCanResapwnAgain(GameObject toPlayer) {
        toPlayer.GetComponent<Health>().inRespawn = false;
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
        if (!isLocalPlayer) {
            return;
        }
        if (healthL <= 0 && !inRespawn) {
            this.gameObject.GetComponent<weaponManager>().DamIDied();
            CmdPlayerDied(playerNumber.currentPlayerNo);
            death = true;
            if (!inRespawn) {
                inRespawn = true;
                CmdRespawn(this.gameObject);
                healthL = maxHealth;
            }

            teleporter.Teleport(respawnLocations[Random.Range(0, respawnLocations.Length)].transform.position);

        }
        //player dying animation player wait for done then reset to give feedback
        if (Healthz <= 0) {
            //uses local
        }
    }
    [Command]
    void CmdTestDamage() {
        Healthz = Healthz - 150;
        GetHit.Play();
    }

    [Command]
    void CmdPlayerDied(int playerNum) {
        deathMessage.deathMessenger(playerNum);

    }

    void OnChangeHealth(int health) {
        if (isLocalPlayer) {
            Healthbar.fillAmount = Map(health, 300, 0, 0, 1);
            healthL = health;
            StartCoroutine(Flash());

        }
    }

    private float Map(float health, float max, float min, float fillMin, float fillMax) {
        return (health - min) * (fillMax - fillMin) / (max - min) + fillMin;
    }


    IEnumerator Flash() {
        //red = new Color(255, 0, 0, 255);
        HudImage.SetActive(false);
        HitMarker.SetActive(true);
        //PlayerHud.color = Color.Lerp(PlayerHud.color, red, 30 * Time.deltaTime);
        yield return new WaitForSeconds(1);
        HudImage.SetActive(true);
        HitMarker.SetActive(false);
    }

}

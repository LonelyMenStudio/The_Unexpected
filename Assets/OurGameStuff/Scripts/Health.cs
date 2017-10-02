﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    private const bool TESTING = true;
    public GameObject respawn;
    private const int maxHealth = 100;
    public Image Healthbar;
    public Image PlayerHud;
    public Image DamageScreenTop;
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
    public bool inRespawn = false;
    public bool death = false;
    public bool getHit = false;
    private GameObject HitMarker;
    //private Color red;
    //private Color reset;
    private bool canSendKill = true;
    public string killMessage = "";
    private string startKillMessage = "You have died!";
    UnityStandardAssets.Characters.FirstPerson.FirstPersonController con;
    private bool turnOffController = false;
    public GameObject deathPop;
    private Text deathText;
    private GameObject deathPopText;
    private int tempDamageFrom;
    public GameObject liveCam;
    public GameObject deathCam;
    private Playeranimations aim;
    private float lowHealthThreshold = 0.33f;
	public AudioSource lowHealthSound;
	public AudioSource deathSound;
	private bool canPlayDeathSound = true;
	private int tempHP;

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
        deathPop = prepPhase.deathPop;
        deathPopText = prepPhase.deathText;
        deathText = deathPopText.GetComponent<Text>();
        barImage = prepPhase.healthObject;
        Healthbar = barImage.GetComponent<Image>();
        HudImage = prepPhase.PlayerHUD;
        HitMarker = prepPhase.HitScreen;
        // HitMarker.SetActive(false);
        DamageScreenTop = HitMarker.GetComponent<Image>();
        PlayerHud = HudImage.GetComponent<Image>();
        playerNumber = this.gameObject.GetComponent<PlayerAssignGet>();
        deathMessage = manager.GetComponent<PlayerManager>();
        teleporter = this.gameObject.GetComponent<NetworkXYZSync>();
        healthL = maxHealth;
        aim = this.gameObject.GetComponent<Playeranimations>();
        //reset = new Color(50, 80, 150, 255);
        //PlayerHud.color = reset;
        con = this.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }

    public void TakeDamage(int[] damageInfo) {
        getHit = true;
        if (!isServer) {
            return;
        }
        int amount = damageInfo[0];
        int damageFrom = damageInfo[1];
        Healthz -= amount;
		//GetHit.Play ();
		if (Healthz <= 0) {
			Healthz = 0;
			if (canSendKill) {
				canSendKill = false;
				sendKill (damageFrom);//150
				tempDamageFrom = damageFrom;
				//killMessage = startKillMessage + "Player " + damageFrom;//because its server!!!
			}
		}
    }
    public void DeathByWater() {
        if (isLocalPlayer) {
            CmdWaterDeath();
            playerNumber.CmdLoseKill();
            killMessage = startKillMessage + "Enviroment";
        }
    }
    [Command]
    void CmdWaterDeath() {
        Healthz = Healthz - Healthz;
        if (Healthz <= 0) {
            Healthz = 0;
        }
    }
    //this only place to icnrease deaths
    [Command]
    void CmdRespawn(GameObject toPlayer) {
        Healthz = maxHealth;
		tempHP = Healthz;
        playerNumber.deaths++;//increase death
		canPlayDeathSound = true;

        //RpcCanRespawnAgain(toPlayer);
        //inRespawn = false;// this was here????
    }
    // [ClientRpc]
    //void RpcCanRespawnAgain(GameObject toPlayer) {
    //  toPlayer.GetComponent<Health>().inRespawn = false;
    // toPlayer.GetComponent<Health>().canSendKill = true;
    //}


    IEnumerator delayRespawn() {
        yield return new WaitForSeconds(1f);
        inRespawn = false;
    }
    IEnumerator canSendKillAgain() {
        yield return new WaitForSeconds(1f);
        canSendKill = true;
    }
    IEnumerator timedRespawn() {
        inRespawn = true;
        turnOffController = true;
        death = true;
        if (deathPop != null) {
            killMessage = startKillMessage;
            deathText.text = killMessage;
            deathPop.SetActive(true);
        }
        liveCam.SetActive(false);
        deathCam.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        this.gameObject.GetComponent<weaponManager>().DamIDied();
        CmdPlayerDied(playerNumber.currentPlayerNo);
        CmdRespawn(this.gameObject);
        if (deathPop != null) {
            deathPop.SetActive(false);
        }
        teleporter.Teleport(respawnLocations[Random.Range(0, respawnLocations.Length)].transform.position);
        StartCoroutine(delayRespawn());
        turnOffController = false;
        deathCam.SetActive(false);
        liveCam.SetActive(true);
        aim.lossWep = true;
    }

    void sendKill(int killerNumber) {
        for (int i = 0; i <= deathMessage.Players.Count; i++) {
            PlayerAssignGet checkPlayer = deathMessage.Players[i].GetComponent<PlayerAssignGet>();
            if (checkPlayer.currentPlayerNo == killerNumber) {
                checkPlayer.CmdIncreaseKill();
                return;
            }
        }
        canSendKillAgain();
        Debug.Log("couldnt give kill");
    }
    // Update is called once per frame
    // only place to call respawn
    void Update() {
        if (!isLocalPlayer) {
            return;
        }
        if (healthL <= 0 && !inRespawn) {
            /*
            inRespawn = true;
            this.gameObject.GetComponent<weaponManager>().DamIDied();
            CmdPlayerDied(playerNumber.currentPlayerNo);
            death = true;
            CmdRespawn(this.gameObject);// call death
            teleporter.Teleport(respawnLocations[Random.Range(0, respawnLocations.Length)].transform.position);
            StartCoroutine(delayRespawn());
            */
            StartCoroutine(timedRespawn());
        }
        //player dying animation player wait for done then reset to give feedback
		SoundCheck ();
        //flash red code
        if (getHit) {
            Color Opaque = new Color(1, 1, 1, 1);
            DamageScreenTop.color = Color.Lerp(DamageScreenTop.color, Opaque, 20 * Time.deltaTime);
            if (DamageScreenTop.color.a >= 0.8) {
                getHit = false;
            }
        }
        if (!getHit) {
            Color Transparent = new Color(1, 1, 1, 0);
            DamageScreenTop.color = Color.Lerp(DamageScreenTop.color, Transparent, 20 * Time.deltaTime);
        }
        if (TESTING && Input.GetKeyDown(KeyCode.O)) {
            CmdTestDamage();
        }
        if (TESTING && Input.GetKeyDown(KeyCode.P)) {
            tpWeapon();
        }
        if (turnOffController) {
            this.gameObject.GetComponent<PrepCheck>().stop = true;
            con.enabled = false;
        } else {
            this.gameObject.GetComponent<PrepCheck>().stop = false;
            //turnOffController = true;
        }
    }
    public void ouch() {
        CmdTestDamage();
    }
    [Command]
    void CmdTestDamage() {
        Healthz = Healthz - 5;
    }

    [Command]
    void CmdPlayerDied(int playerNum) {
        deathMessage.deathMessenger(playerNum);

    }

	void SoundCheck() {
		if (Healthz <= 0) {
			lowHealthSound.Stop ();
			if (canPlayDeathSound) {
				if (!isServer) {
					CmdPlayDeathAudio();
				} else {
					RpcPlayDeathAudio();
				}
				canPlayDeathSound = false;
			} 
			//uses local
		} else if (Healthz < (maxHealth * lowHealthThreshold)) {
			if (!lowHealthSound.isPlaying) {
				//print ("playing");
				lowHealthSound.Play ();
			}
		} else {
			//print ("stop");
			lowHealthSound.Stop ();
		}
		if (Healthz < tempHP) {
			GetHit.Play ();
		}
		tempHP = Healthz;
	}

    void OnChangeHealth(int health) {
        if (isLocalPlayer) {
            Healthbar.fillAmount = Map(health, maxHealth, 0, 0, 1);
            healthL = health;
            //   StartCoroutine(Flash());

        }
    }

    private float Map(float health, float max, float min, float fillMin, float fillMax) {
        return (health - min) * (fillMax - fillMin) / (max - min) + fillMin;
    }


    /*  IEnumerator Flash() {
          //red = new Color(255, 0, 0, 255);
          HudImage.SetActive(false);
          HitMarker.SetActive(true);
          //PlayerHud.color = Color.Lerp(PlayerHud.color, red, 30 * Time.deltaTime);
          yield return new WaitForSeconds(1);
          HudImage.SetActive(true);
          HitMarker.SetActive(false);
      }*/

    public void CrystalHeal(int amount) {
        if (!isLocalPlayer) {
            return;
        }
        CmdEnviromentEffect(false, amount, 0);
    }
    public void BarrelDamage(int[] amount) {
        if (!isLocalPlayer) {
            return;
        }
        CmdEnviromentEffect(true, amount[0], amount[1]);
    }
    [Command]
    void CmdEnviromentEffect(bool damage, int amount, int from) {
        if (damage) {
            Healthz = Healthz - amount;
            if (Healthz <= 0) {
                Healthz = 0;
                if (canSendKill) {
                    canSendKill = false;
                    sendKill(from);//150
                }
            }
        } else {
            Healthz = Healthz + amount;
            if (Healthz > maxHealth) {
                Healthz = maxHealth;
            }
        }

    }

    void tpWeapon() {
        for (int i = 0; i < manager.GetComponent<PlayerManager>().droppedWeapons.Count; i++) {
            manager.GetComponent<PlayerManager>().droppedWeapons[i].transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 4, this.transform.position.z);

        }
    }

	[Command]
	void CmdPlayDeathAudio() {
		RpcPlayDeathAudio();
	}

	[ClientRpc]
	void RpcPlayDeathAudio() {
		deathSound.Play();
	}
}

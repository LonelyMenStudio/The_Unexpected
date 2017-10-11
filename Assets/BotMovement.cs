using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
public class BotMovement : MonoBehaviour {

    [SerializeField]
    Transform [] destination;


   // public bool runIK = false;
    [Space(20)]
    public Transform leftUpperArm;
    public Transform leftForearm;
    public Transform leftHand2;
    public Transform leftElbow;
    public Transform leftTarget;
    public Vector3 leftUpperArm_OffsetRotation;
    public Vector3 leftForearm_OffsetRotation;
    public Vector3 leftHand_OffsetRotation;
    public bool lefHandMatchesTargetRotation = true;
    public bool leftDebug;
    float leftAngle;
    float leftUpperArm_Length;
    float leftForearm_Length;
    float leftArm_Length;
    float leftTargetDistance;
    float leftAdyacent;
    [Space(50)]
    public Transform upperArm;
    public Transform forearm;
    public Transform hand;
    public Transform elbow;
    public Transform target;
    public Vector3 uppperArm_OffsetRotation;
    public Vector3 forearm_OffsetRotation;
    public Vector3 hand_OffsetRotation;
    public bool handMatchesTargetRotation = true;
    public bool debug;
    float angle;
    float upperArm_Length;
    float forearm_Length;
    float arm_Length;
    float targetDistance;
    float adyacent;
    //private weaponManager weapon;
    private int currentWeapon;
    public GameObject R1, L1, DL, DR;
    private bool onceAfterStop = false;
    //private Playeranimations aim;

    public Transform rightHandObj = null;
    public Animator animator;
   // public bool ikActive = true;
    private GameObject Variables;
    private VariablesScript ManagerGet;
    Animator animatorz;
    public GameObject Rig;
    private bool Changelocation = true;
    NavMeshAgent navMeshAgent;
    private int i;
    private Vector3 tragetVector;
    public float distance2;
    public float Timegun;
    private float BotGetsGunAT = 60;
    public GameObject BotsCrappygun;
    private bool hasgun = false;
    private bool playerfound = false;
    private float distancefromplayer;
    private GameObject Manager;
    private PlayerManager pManager;
    private List<GameObject> Playerz;
    public GameObject botcam;
    private GameObject childRoot;
    public float counter;
    private float delayTime = 0.5f;
    //public GameObject shotParticlegb;
    public ParticleSystem shotParticle;
    public float BotHealth = 100;
    public GameObject[] spawn;
    Quaternion Notaimrot;
    Vector3 Notaimpos;
    public GameObject righthand;
    public float TimetolosePlayer;
    private bool standstill = false;

    public AvatarIKGoal rightHand = AvatarIKGoal.RightHand;
    public AvatarIKGoal leftHand = AvatarIKGoal.LeftHand;

	public AudioSource ded;
	private bool isDed = false;
	public AudioSource clem;
	private float[] tempLocation = new float[3];
    public PrepPhase killbot;
    public GameObject deathicon;
    private ScoreScreen stats;
    public int Botnum;
    public int botkills;
    private int botdeaths;
    private bool botdied = true;

    // Use this for initialization
    void Start () {
        
        Variables = GameObject.FindWithTag("Start");
        ManagerGet = Variables.GetComponent<VariablesScript>();
        Manager = ManagerGet.variables;
        pManager = Manager.GetComponent<PlayerManager>();
        killbot = Manager.GetComponent<PrepPhase>();
        stats = killbot.playerStats.GetComponent<ScoreScreen>();
        deathicon = killbot.killGetMessage;
        Playerz = pManager.Players;
        BotsCrappygun.SetActive(false);
        animatorz = Rig.GetComponent<Animator>();
        animator = GetComponent<Animator>();
        i = Random.Range(0, destination.Length);
        tragetVector = destination[i].transform.position;
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        botcam = transform.Find("BotCam").gameObject;
        if (navMeshAgent == null) {
            Debug.LogError("The nav mesh agent compnent is not attached to" + gameObject.name);
        }else {
            SetDestination();
        }
		tempLocation = new float[] { 0f, 0f, 0f };
	}
    public void TakeDamage(float Damage) {
        BotHealth -= Damage;
        playerfound = true;
        TimetolosePlayer = counter;
        if (BotHealth <= 0) {
            BotHealth = 0;
            
            if (botdied) {
                botdeaths++;
                PlayerAssignGet playerscore = Playerz[0].GetComponent<PlayerAssignGet>();
                playerscore.kills++;
                botdied = false;
            }
            deathicon.SetActive(true);
            animatorz.Play("Death");
			if (!isDed) {
				ded.Play ();
				isDed = true;
			}

            StartCoroutine(died());
            
        }
    }
    private void SetDestination() {
        if (!playerfound) {
            distance2 = Vector3.Distance(transform.position, destination[i].transform.position);
            if (destination != null) {
                if (distance2 <= 5) {
                    i = Random.Range(0, destination.Length);

                }
                tragetVector = destination[i].transform.position;

                navMeshAgent.SetDestination(tragetVector);
            }
        } else {
            distance2 = Vector3.Distance(transform.position, Playerz[0].transform.position);
            if (Playerz != null) {
                if (distance2 <= 5) {


                } else {
                    tragetVector = Playerz[0].transform.position;

                    navMeshAgent.SetDestination(tragetVector);
                }
            }
        }
    }
    void OnAnimatorIK() {
        if (hasgun) {
            if (rightHandObj != null) {
                animator.SetIKPositionWeight(rightHand, 1);
                animator.SetIKRotationWeight(rightHand, 1);
                animator.SetIKPositionWeight(leftHand, 1);
                animator.SetIKRotationWeight(leftHand, 1);
                Notaimpos = righthand.transform.position;
                Notaimrot = righthand.transform.rotation;
                    HandStuff(Notaimpos, Notaimrot);
                
            }
        }
    }

    void Update() {
        int Pcount = pManager.Players.Count;
        if (Pcount > 1) {
            Destroy(gameObject);
        }
        if (!standstill) {
            animatorz.SetBool("isWalking", true);
        } else if (standstill) {
            animatorz.SetBool("isWalking", false);
        }
        if (hasgun) {
            animatorz.SetBool("HasWep", true);
        }

        Timegun += Time.deltaTime;
        if (Timegun >= BotGetsGunAT) {
            BotsCrappygun.SetActive(true);
            hasgun = true;
        }
        counter += Time.deltaTime;
        int playercount;
        playercount = Playerz.Count;
        /*  if (playercount != 1) {
              Destroy(gameObject);
          }*/
        if (!hasgun) {
            SetDestination();
        } else {
            SetDestination();
            distancefromplayer = Vector3.Distance(transform.position, Playerz[0].transform.position);
            if (distancefromplayer <= 100) {
                playerfound = true;
                TimetolosePlayer = counter;
            } else if (distancefromplayer > 150) {
                playerfound = false;
            }

            if (playerfound) {
                if (distancefromplayer > 1) {
                    SetDestination();
                    standstill = false;
                } else {
                    standstill = true;
                }
                lookatplayer();
                int currentplayerhealth = Playerz[0].GetComponent<Health>().Healthz;
                RaycastHit hit2;
                if ((Physics.Raycast(botcam.transform.position, botcam.transform.forward, out hit2) && counter > delayTime && currentplayerhealth > 0) && !isDed) {
                    if (hit2.transform.tag == "Player") {
                        counter = 0;
                        Health call = hit2.transform.gameObject.GetComponent<Health>();
                        call.ouch(Botnum);
                        //Instantiate(shotParticle, botcam.transform.position, botcam.transform.rotation);
                        shotParticle.Play();
                        clem.Play();
                        //shoot the player

                    }
                }
            } else {

            }
        }
        if (tempLocation[0] != transform.position.x || tempLocation[1] != transform.position.y || tempLocation[2] != transform.position.z) {
            tempLocation[0] = transform.position.x;
            tempLocation[1] = transform.position.y;
            tempLocation[2] = transform.position.z;
            StartCoroutine(GetComponent<footsteps>().BotWalk());
        }
        if (Playerz.Count == 1) {
            if (Botnum == 1) {

                botkills = Playerz[0].GetComponent<Health>().botkills;
                stats.playerNames[1].text = "Bot 1";
                stats.playerKills[1].text = "" + botkills;
                stats.playerDeaths[1].text = "" + botdeaths;

            } else if (Botnum == 2) {
                botkills = Playerz[0].GetComponent<Health>().botkills2;
                stats.playerNames[2].text = "Bot 2";
                stats.playerKills[2].text = "" + botkills;
                stats.playerDeaths[2].text = "" + botdeaths;
            }
            if (botdied == false) {
                StartCoroutine(deathstall());


            }
        }
    }
                void lookatplayer() {
        botcam.transform.rotation = Quaternion.Slerp(botcam.transform.rotation, Quaternion.LookRotation(Playerz[0].transform.position - botcam.transform.position), 2*Time.deltaTime);
                 //   Quaternion rotation = Quaternion.LookRotation(Playerz[0].transform.position - botcam.transform.position);
                 //  transform.rotation = Quaternion.Slerp(botcam.transform.rotation, rotation, 3 * Time.deltaTime);
    }
    void HandStuff(Vector3 psi, Quaternion rot) {
        SwitchWeapon(psi, rot);

    }

    void SwitchWeapon(Vector3 pos, Quaternion rotation) {
        rightHandObj.position =  pos;
        rightHandObj.rotation = rotation;

    }
    IEnumerator deathstall() {
        yield return new WaitForSeconds(3);
        botdied = true;
    }
    IEnumerator died() {
        
        yield return new WaitForSeconds(1.5f);
        int indexspawn = Random.Range(0, spawn.Length);
        transform.position = spawn[indexspawn].transform.position;
        BotHealth = 100;
		isDed = false;
        deathicon.SetActive(false);
    }
    // Update is called once per frame
    void LateUpdate() {
        if (!hasgun) {
            if (onceAfterStop) {
                target = DR.transform;
                leftTarget = DL.transform;
                LeftAction();
                RightAction();
                onceAfterStop = false;
            }
            return;
        }
        onceAfterStop = true;
            if (hasgun) {
                target = R1.transform;
                leftTarget = L1.transform;
            }
        
        LeftAction();
        RightAction();
    }
    void LeftAction() {
        if (leftUpperArm != null && leftForearm != null && leftHand2 != null && leftElbow != null && leftTarget != null) {
            leftUpperArm.LookAt(leftTarget, leftElbow.position - leftUpperArm.position);
            leftUpperArm.Rotate(leftUpperArm_OffsetRotation);

            Vector3 leftCross = Vector3.Cross(leftElbow.position - leftUpperArm.position, leftForearm.position - leftUpperArm.position);



            leftUpperArm_Length = Vector3.Distance(leftUpperArm.position, leftForearm.position);
            leftForearm_Length = Vector3.Distance(leftForearm.position, leftHand2.position);
            leftArm_Length = leftUpperArm_Length + leftForearm_Length;
            leftTargetDistance = Vector3.Distance(leftUpperArm.position, leftTarget.position);
            leftTargetDistance = Mathf.Min(leftTargetDistance, leftArm_Length - leftArm_Length * 0.001f);

            leftAdyacent = ((leftUpperArm_Length * leftUpperArm_Length) - (leftForearm_Length * leftForearm_Length) + (leftTargetDistance * leftTargetDistance)) / (2 * leftTargetDistance);

            leftAngle = Mathf.Acos(leftAdyacent / leftUpperArm_Length) * Mathf.Rad2Deg;

            leftUpperArm.RotateAround(leftUpperArm.position, leftCross, -leftAngle);

            leftForearm.LookAt(leftTarget, leftCross);
            leftForearm.Rotate(leftForearm_OffsetRotation);

            if (lefHandMatchesTargetRotation) {
                leftHand2.rotation = leftTarget.rotation;
                leftHand2.Rotate(leftHand_OffsetRotation);
            }

            if (leftDebug) {
                if (leftForearm != null && leftElbow != null) {
                    Debug.DrawLine(leftForearm.position, leftElbow.position, Color.blue);
                }

                if (leftUpperArm != null && leftTarget != null) {
                    Debug.DrawLine(leftUpperArm.position, leftTarget.position, Color.red);
                }
            }

        }
    }

    void RightAction() {
        if (upperArm != null && forearm != null && hand != null && elbow != null && target != null) {
            upperArm.LookAt(target, elbow.position - upperArm.position);
            upperArm.Rotate(uppperArm_OffsetRotation);

            Vector3 cross = Vector3.Cross(elbow.position - upperArm.position, forearm.position - upperArm.position);



            upperArm_Length = Vector3.Distance(upperArm.position, forearm.position);
            forearm_Length = Vector3.Distance(forearm.position, hand.position);
            arm_Length = upperArm_Length + forearm_Length;
            targetDistance = Vector3.Distance(upperArm.position, target.position);
            targetDistance = Mathf.Min(targetDistance, arm_Length - arm_Length * 0.001f);

            adyacent = ((upperArm_Length * upperArm_Length) - (forearm_Length * forearm_Length) + (targetDistance * targetDistance)) / (2 * targetDistance);

            angle = Mathf.Acos(adyacent / upperArm_Length) * Mathf.Rad2Deg;

            upperArm.RotateAround(upperArm.position, cross, -angle);

            forearm.LookAt(target, cross);
            forearm.Rotate(forearm_OffsetRotation);

            if (handMatchesTargetRotation) {
                hand.rotation = target.rotation;
                hand.Rotate(hand_OffsetRotation);
            }

            if (debug) {
                if (forearm != null && elbow != null) {
                    Debug.DrawLine(forearm.position, elbow.position, Color.blue);
                }

                if (upperArm != null && target != null) {
                    Debug.DrawLine(upperArm.position, target.position, Color.red);
                }
            }

        }

    }

    void LeftOnDrawGizmos() {
        if (leftDebug) {
            if (leftUpperArm != null && leftElbow != null && leftHand2 != null && leftTarget != null && leftElbow != null) {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(leftUpperArm.position, leftForearm.position);
                Gizmos.DrawLine(leftForearm.position, leftHand2.position);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(leftUpperArm.position, leftTarget.position);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(leftForearm.position, leftElbow.position);
            }
        }
    }

    void OnDrawGizmos() {
        if (debug) {
            if (upperArm != null && elbow != null && hand != null && target != null && elbow != null) {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(upperArm.position, forearm.position);
                Gizmos.DrawLine(forearm.position, hand.position);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(upperArm.position, target.position);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(forearm.position, elbow.position);
            }
        }
    }
}

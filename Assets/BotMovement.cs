using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BotMovement : MonoBehaviour {

    [SerializeField]
    Transform [] destination;

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
    public Camera botcam;
    private GameObject childRoot;
    private float counter;
    private float delayTime;

    // Use this for initialization
    void Start () {
        Variables = GameObject.FindWithTag("Start");
        ManagerGet = Variables.GetComponent<VariablesScript>();
        Manager = ManagerGet.variables;
        pManager = Manager.GetComponent<PlayerManager>();
        Playerz = pManager.Players;
        BotsCrappygun.SetActive(false);
        animatorz = Rig.GetComponent<Animator>();
        animatorz.SetBool("isWalking", true);
        i = Random.Range(0, destination.Length);
        tragetVector = destination[i].transform.position;
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        childRoot = transform.Find("FirstPersonCharacter").gameObject;

        if (navMeshAgent == null) {
            Debug.LogError("The nav mesh agent compnent is not attached to" + gameObject.name);
        }else {
            SetDestination();
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
    void Update() {
        counter += Time.deltaTime;
        int playercount;
        playercount = Playerz.Count;
        /*  if (playercount != 1) {
              Destroy(gameObject);
          }*/
        if (!hasgun) {
            SetDestination();
        } else {

            distancefromplayer = Vector3.Distance(transform.position, Playerz[0].transform.position);
            if (distancefromplayer <= 100) {
                playerfound = true;
            }
            if (playerfound) {
                lookatplayer();
                SetDestination();
                RaycastHit hit2;
                if (Physics.Raycast(botcam.transform.position, childRoot.transform.forward, out hit2) && counter > delayTime) {
                    if (hit2.transform.tag == "Player") {
                        counter = 0;
                        Debug.Log("I am shooting you! you just don't know it");
                        //shoot the player
                    }

                    Timegun += Time.deltaTime;

                    if (Timegun >= BotGetsGunAT) {
                        BotsCrappygun.SetActive(true);
                        hasgun = true;
                    }
                }
            }
        }
    }
                void lookatplayer() {
                    Quaternion rotation = Quaternion.LookRotation(Playerz[0].transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                }

}

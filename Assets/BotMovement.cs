using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BotMovement : MonoBehaviour {

    [SerializeField]
    Transform [] destination;

    Animator animatorz;
    public GameObject Rig;
    private bool Changelocation = true;
    NavMeshAgent navMeshAgent;
    private int i;
    private Vector3 tragetVector;
    public float distance2;
    public float Timegun; 
    // Use this for initialization
    void Start () {
        animatorz = Rig.GetComponent<Animator>();
        animatorz.SetBool("isWalking", true);
        i = Random.Range(0, destination.Length);
        tragetVector = destination[i].transform.position;
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (navMeshAgent == null) {
            Debug.LogError("The nav mesh agent compnent is not attached to" + gameObject.name);
        }else {
            SetDestination();
        }
	}
	
    private void SetDestination() {
        distance2 = Vector3.Distance(transform.position, destination[i].transform.position);
        if ( destination != null) {
            if ( distance2 <= 5) {
                i = Random.Range(0, destination.Length);
                
            }
            tragetVector = destination[i].transform.position;
            navMeshAgent.SetDestination(tragetVector);
        }
    }
    void Update() {
        SetDestination();
        Timegun += Time.deltaTime;
    }
}

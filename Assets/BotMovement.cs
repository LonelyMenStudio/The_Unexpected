using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BotMovement : MonoBehaviour {

    [SerializeField]
    Transform [] destination;

    private bool Changelocation = true;
    NavMeshAgent navMeshAgent;
    public int i;
	// Use this for initialization
	void Start () {
        i = Random.Range(0, destination.Length);
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (navMeshAgent == null) {
            Debug.LogError("The nav mesh agent compnent is not attached to" + gameObject.name);
        }else {
            SetDestination();
        }
	}
	
    private void SetDestination() {
       
        if ( destination != null) {
            if (Changelocation == true) {
                i = Random.Range(0, destination.Length);
                Changelocation = false;
            }

            Vector3 tragetVector = destination[i].transform.position;
            navMeshAgent.SetDestination(tragetVector);
        }
    }
}

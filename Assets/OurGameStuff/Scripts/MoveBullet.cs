using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour {

    GameObject player;
    public float speed = 3f;
    public float damage = 20f;
    
    void Awake() {
        Destroy(gameObject, 5.0f);
    }
    // Update is called once per frame
    void Update () {
        transform.Translate(0, 0, speed);
	}

    void OnTriggerEnter(Collider other) {
        //other.gameObject.GetComponent<PlayerStats>().takeDamage(damage);
        other.gameObject.GetComponent<Enemy>().TakeDamage(damage);


    }
}

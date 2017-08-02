using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float startHealth = 30;
    public static float curhealthz = 0;


    void Start() {
        curhealthz = startHealth;
    }


    public void TakeDamage(float amount) {
        curhealthz -= amount;
        if (curhealthz <= 0) {
            curhealthz = 0;
            Destroy(gameObject);
        }
    }
   /*void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Bullet") {
            TakeDamage(20);
        }
    }*/
   
}

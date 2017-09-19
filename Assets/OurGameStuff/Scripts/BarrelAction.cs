using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BarrelAction : NetworkBehaviour {

    [SyncVar]
    public float barrelHealth = 5;
    [SyncVar]
    private bool done = false;
    [SyncVar]
    private int tempDamageFrom;
    public ParticleSystem Explosion;
    private bool barrelDestoryed = false;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (barrelHealth <= 0 && !barrelDestoryed && done) {
            Destroy(this.gameObject.GetComponent<MeshCollider>());
            Destroy(this.gameObject.GetComponent<MeshRenderer>());
            barrelDestoryed = true;
            this.gameObject.GetComponent<EnvBarrelDamage>().damageFrom(tempDamageFrom);
            this.gameObject.GetComponent<EnvBarrelDamage>().barrelHasBeenDestoryed = barrelDestoryed;
            Explosion.Play();
        }

    }
    //bad name but hey
    public void DamageCrystal(int[] damage) {
        if (!isServer) {
            return;
        }
        barrelHealth = barrelHealth - damage[0];
        if (barrelHealth <= 0) {
            tempDamageFrom = damage[1];
            done = true;
        }
    }
}

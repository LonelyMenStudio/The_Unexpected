using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BarrelAction : NetworkBehaviour {

    [SyncVar]
    public float barrelHealth = 5;
    public ParticleSystem Explosion;
    private bool barrelDestoryed = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (barrelHealth <= 0 && !barrelDestoryed) {
            Destroy(this.gameObject.GetComponent<MeshCollider>());
            Destroy(this.gameObject.GetComponent<MeshRenderer>());
            barrelDestoryed = true;
            this.gameObject.GetComponent<EnvBarrelDamage>().barrelHasBeenDestoryed = barrelDestoryed;
            Explosion.Play();
        }

    }

    public void DamageCrystal(float damage) {
        if (!isServer) {
            return;
        }
        barrelHealth = barrelHealth - damage;
    }
}

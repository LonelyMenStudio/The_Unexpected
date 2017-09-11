using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BarrelAction : NetworkBehaviour {

    [SyncVar]
    public float barrelHealth = 50;

    private bool barrelDestoryed = false;
    public GameObject childTrigger;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (barrelHealth <= 0 && !barrelDestoryed) {
            Destroy(this.gameObject.GetComponent<MeshCollider>());
            Destroy(this.gameObject.GetComponent<MeshRenderer>());
            barrelDestoryed = true;
            if (childTrigger != null) {
                childTrigger.GetComponent<EnvCrystalHeal>().crystalHasBeenDestoryed = barrelDestoryed;
            }
            //Instantiate particle effect
        }

    }

    public void DamageCrystal(float damage) {
        if (!isServer) {
            return;
        }
        barrelHealth = barrelHealth - damage;
    }
}

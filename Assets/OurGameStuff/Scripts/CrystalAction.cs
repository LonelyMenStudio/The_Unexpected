using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CrystalAction : NetworkBehaviour {

    [SyncVar]
    public float crystalHealth = 50;

    private bool crystalDestoryed = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (crystalHealth <= 0 && !crystalDestoryed) {
            Destroy(this.gameObject.GetComponent<MeshCollider>());
            Destroy(this.gameObject.GetComponent<MeshRenderer>());
            crystalDestoryed = true;
            this.gameObject.GetComponent<EnvCrystalHeal>().crystalHasBeenDestoryed = crystalDestoryed;
            //Instantiate particle effect
        }
        
    }

    public void DamageCrystal(float damage) {
        if (!isServer) {
            return;
        }
        crystalHealth = crystalHealth - damage;
    }
}

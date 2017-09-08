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
            //Destroy(this.gameObject);//should be ok actually iffy  might need some sort of network destory because well reasonsst
            //Instantiate (on network) particle effect
        }
    }

    public void DamageCrystal(float damage) {
        if (!isServer) {
            return;
        }
        crystalHealth = crystalHealth - damage;
    }
}

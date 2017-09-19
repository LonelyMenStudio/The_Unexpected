using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CrystalAction : NetworkBehaviour {

    [SyncVar]
    public int crystalHealth = 5;
    public ParticleSystem Explosion;
    private bool crystalDestoryed = false;
    private AudioSource boom;

    // Use this for initialization
    void Start() {
        boom = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (crystalHealth <= 0 && !crystalDestoryed) {
            Destroy(this.gameObject.GetComponent<MeshCollider>());
            Destroy(this.gameObject.GetComponent<MeshRenderer>());
            if (!isServer) {
                CmdPlayAudio();
            } else {
                RpcPlayAudio();
            }
            crystalDestoryed = true;
            this.gameObject.GetComponent<EnvCrystalHeal>().crystalHasBeenDestoryed = crystalDestoryed;
            Explosion.Play();
        }

    }

    public void DamageCrystal(int[] damage) {
        if (!isServer) {
            return;
        }
        crystalHealth = crystalHealth - damage[0];
    }

    [Command]
    void CmdPlayAudio() {
        RpcPlayAudio();
    }

    [ClientRpc]
    void RpcPlayAudio() {
        try {
            boom.Play();
        } catch {
            return;
        }
    }
}

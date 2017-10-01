using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class weaponSettings : NetworkBehaviour {

    [SyncVar]
    public int currentAmmo;

    [SyncVar]
    public int maxAmmo;

    [SyncVar]
    public int playerNo;

    private GameObject Variables;
    private VariablesScript ManagerGet;
    private GameObject Manager;
    private PlayerManager pManager;

    public bool isCrap = false;

    // Use this for initialization
    void Start() {
        if (!isCrap) {
            Variables = GameObject.FindWithTag("Start");
            ManagerGet = Variables.GetComponent<VariablesScript>();
            Manager = ManagerGet.variables;
            pManager = Manager.GetComponent<PlayerManager>();
            pManager.droppedWeapons.Add(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManagerSelf : NetworkBehaviour {

    private PlayerManager pManager;
    private GameObject manager;
    public GameObject Variables;
    private VariablesScript ManagerGet;

    void Awake() {
        Variables = GameObject.FindWithTag("Start");
    }

    // Use this for initialization
    void Start () {
        ManagerGet = Variables.GetComponent<VariablesScript>();
        manager = ManagerGet.variables;
        pManager = manager.GetComponent<PlayerManager>();
        AddSelf();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddSelf() {
        pManager.Players.Add(this.gameObject);
    }
    public void DropWeaponFromList(GameObject weapon) {
        pManager.droppedWeapons.Remove(weapon);
    }
       
}

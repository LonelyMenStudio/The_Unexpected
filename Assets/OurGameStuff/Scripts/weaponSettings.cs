using UnityEngine;
using System.Collections;

public class weaponSettings : MonoBehaviour {

    public int currentAmmo;
    public int maxAmmo;
    public int playerNo;

    private GameObject Variables;
    private VariablesScript ManagerGet;
    private GameObject Manager;
    private PlayerManager pManager;


    // Use this for initialization
    void Start () {
        Variables = GameObject.FindWithTag("Start");
        ManagerGet = Variables.GetComponent<VariablesScript>();
        Manager = ManagerGet.variables;
        pManager = Manager.GetComponent<PlayerManager>();
        pManager.droppedWeapons.Add(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void removeSelfFromList() {
        pManager.droppedWeapons.Remove(this.gameObject);
    }
}

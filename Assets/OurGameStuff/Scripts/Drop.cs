using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour {

    public GameObject dropItem;
    public GameObject currentItem;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("e")) {
            Instantiate(dropItem, transform.position, transform.rotation);

            DropI();
        }
	}
    void DropI() {
        currentItem.SetActive(false);
    }
}

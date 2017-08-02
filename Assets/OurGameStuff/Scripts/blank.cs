using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class blank : NetworkBehaviour {

    private GameObject foundChild;

    void Start() {
        //print("one");
        
        foundChild = this.transform.GetChild(0).gameObject;
        if (!isLocalPlayer) {
            //foundChild.gameObject.GetComponent<Camera>().enabled = true;
            //foundChild.gameObject.GetComponent<AudioListener>().enabled = true;
            //print("failed");
            Destroy(this.transform.GetChild(1).gameObject.GetComponent("FlareLayer"));//transform.FindChild("FirstPersonCharacter")
            Destroy(this.transform.GetChild(0).gameObject.GetComponent("FlareLayer"));
            Destroy(this.transform.GetChild(0).gameObject.GetComponent<FlareLayer>());
            Destroy(this.transform.GetChild(1).gameObject.GetComponent("Camera"));//transform.FindChild("FirstPersonCharacter")
            Destroy(this.transform.GetChild(0).gameObject.GetComponent("Camera"));
            Destroy(this.transform.GetChild(0).gameObject.GetComponent<Camera>());
            Destroy(this.transform.GetChild(1).gameObject.GetComponent("AudioListener"));//transform.FindChild("FirstPersonCharacter")
            Destroy(this.transform.GetChild(0).gameObject.GetComponent("AudioListener"));
            Destroy(this.transform.GetChild(0).gameObject.GetComponent<AudioListener>());

            //Destroy(foundChild.gameObject.GetComponent("AudioListener"));
            foundChild.gameObject.tag = "Untagged";
            Destroy(this);
            return;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}

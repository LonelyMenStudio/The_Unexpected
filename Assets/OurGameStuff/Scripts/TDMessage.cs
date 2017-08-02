using UnityEngine;
using System.Collections;

public class TDMessage : MonoBehaviour {

    public GameObject relay;

	public void TakeDamage(int dam) {
        print("OW");
        relay.SendMessage("TakeDamage", dam, SendMessageOptions.RequireReceiver);
    }
}

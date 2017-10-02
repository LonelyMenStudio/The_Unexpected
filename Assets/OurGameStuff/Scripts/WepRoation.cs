using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WepRoation : MonoBehaviour {

    private bool floatup;
    private float weprotatio = 0.2f;
    private float smooth = 2.0f;
    private float tiltAngle = 5.0f;
    private float floatingStrength = 0.03f;
    public GameObject gun;
	// Use this for initialization
	void Start () {
        floatup = false;


    }
	
	// Update is called once per frame
	void Update () {
        weprotatio += 0.4f;
        if (floatup) {
            floatingup();
        }else if(!floatup){
            floatingdown();
        }
        float tiltAroundZ = Input.GetAxis("Horizontal");
        float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
        Quaternion target = Quaternion.Euler(tiltAroundX-90, 0, tiltAroundZ + weprotatio);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }
    void floatingup() {
        float currentx = gun.transform.position.x;
        float currenty = gun.transform.position.y;
        float currentz = gun.transform.position.z;
        gun.transform.position = new Vector3(currentx, currenty + ((float)Mathf.Sin(Time.time) * floatingStrength), currentz);
        StartCoroutine(Floating());
        floatup = false;
    }
    void floatingdown() {
        float currentx = gun.transform.position.x;
        float currenty = gun.transform.position.y;
        float currentz = gun.transform.position.z;
        //transform.position = new Vector3(currentx, currenty - ((float)Mathf.Sin(Time.time) * floatingStrength), currentz);
        StartCoroutine(Floating());
        floatup = true;
    }
    IEnumerator Floating() {
        yield return new WaitForSeconds(2);
    }

}

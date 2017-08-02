using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camrotation : MonoBehaviour {
    public float smooth = 2.0F;
    public float tiltAngle = 130;
    public float increase = 2f;
    // Use this for initialization
    void Start() {
        gameObject.SetActive(false);

    }
	// Update is called once per frame
	void Update () {
        /* float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
         float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
         Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
         transform.rotation = Quaternion.Slerp(transform.rotation , target, Time.deltaTime * smooth);*/
        transform.Rotate(Vector3.up * Time.deltaTime * smooth, Space.World);
    }
}

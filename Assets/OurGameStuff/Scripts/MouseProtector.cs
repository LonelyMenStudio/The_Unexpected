using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseProtector : MonoBehaviour {

    // Update is called once per frame
    void Update() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

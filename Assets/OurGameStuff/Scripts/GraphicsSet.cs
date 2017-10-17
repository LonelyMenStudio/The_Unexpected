using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSet : MonoBehaviour {

    private int inputX;
    private int inputY;
    private bool fullscreen = true;

    public GameObject xGet;
    public GameObject yGet;

    public void setGraphics() {
        if (xGet.GetComponent<InputField>().textComponent.text != null && yGet.GetComponent<InputField>().textComponent.text != null) {
            inputX = int.Parse(xGet.GetComponent<InputField>().textComponent.text);
            inputY = int.Parse(yGet.GetComponent<InputField>().textComponent.text);
        }
        Screen.SetResolution(inputX, inputY, fullscreen);
    }
}

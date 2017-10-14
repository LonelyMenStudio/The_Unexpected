using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManual : MonoBehaviour {

    public GameObject button;
    public GameObject title;
    public GameObject create;
    public GameObject input;
    public GameObject join;

    public void displayManual() {
        title.SetActive(true);
        create.SetActive(true);
        input.SetActive(true);
        join.SetActive(true);
        button.SetActive(false);
    }
}

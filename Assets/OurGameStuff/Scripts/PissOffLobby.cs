using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PissOffLobby : MonoBehaviour {

    private GameObject Lobby;

    void Start() {
        Lobby = GameObject.FindWithTag("NetWork");
    }
    // Use this for initialization
    public void loadscene(int sceneIndex) {
        Lobby.SetActive(false);
        SceneManager.LoadScene(sceneIndex);

    }
}

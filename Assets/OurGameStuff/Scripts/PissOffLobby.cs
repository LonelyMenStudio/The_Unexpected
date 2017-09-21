using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PissOffLobby : MonoBehaviour {

    public GameObject Lobby;

    void Start() {
        //Lobby = GameObject.FindWithTag("NetWork");
    }
    // Use this for initialization
    public void loadscene(int sceneIndex) {
        /*
        GameObject lobby = GameObject.Find("LobbyNetworkManagr");
        if (lobby != null) {
            Destroy(lobby);
        }
        GameObject lobbyP = GameObject.Find("Lobby Player (Clone)");
        if (lobbyP != null) {
            Destroy(lobbyP);
        }
        Lobby.SetActive(false);
        */
        SceneManager.LoadScene(sceneIndex);


    }
    public void LoadMenu() {
        //Lobby = GameObject.Find("LobbyManager");
        
        SceneManager.LoadScene(0);
        Destroy(Lobby);
    }
}

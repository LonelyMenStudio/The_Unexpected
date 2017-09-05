using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour {
    /* public bool makeitwork = false;
     void Start() {
         GameObject[] waypointArray;
         waypointArray = GameObject.FindGameObjectsWithTag("Network");
         if ( waypointArray.Length != 0) {
             makeitwork = true;
         }
     }*/
    public void loadscene(int sceneIndex) {

        // if (makeitwork) {
        //     GameObject Lobby = GameObject.FindWithTag("NetWork");
        //     Lobby.SetActive(true);
        //       SceneManager.LoadScene(sceneIndex);
        //  } else {
        SceneManager.LoadScene(sceneIndex);
        //    }
    }
}

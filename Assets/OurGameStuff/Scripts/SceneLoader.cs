using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;


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
        if (sceneIndex == 1) {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Going to Lobby");
        }
        else if (sceneIndex == 0) {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Going to Menu");
        }
        // if (makeitwork) {
        //     GameObject Lobby = GameObject.FindWithTag("NetWork");
        //     Lobby.SetActive(true);
        //       SceneManager.LoadScene(sceneIndex);
        //  } else {
        SceneManager.LoadScene(sceneIndex);
        //    }
    }
}

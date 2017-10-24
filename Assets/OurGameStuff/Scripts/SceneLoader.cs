using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;

public class SceneLoader : MonoBehaviour {
    public void loadscene(int sceneIndex) {
        if (sceneIndex == 1) {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Going to Lobby");
        }
        else if (sceneIndex == 0) {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Going to Menu");
        }
        SceneManager.LoadScene(sceneIndex);
    }
}

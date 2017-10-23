using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToggleAnalytics : MonoBehaviour {


    public GameObject analytics; 
	
    void start() {

        DontDestroyOnLoad(this.gameObject);
      //  analytics = GameObject.FindWithTag("GA");
        /*if(analytics.Length > 1) {
            Destroy(analytics.gameObject);

        }*/
    }

    public void Toggle_Analytics(bool state){
        analytics.gameObject.SetActive(state);
    }
}

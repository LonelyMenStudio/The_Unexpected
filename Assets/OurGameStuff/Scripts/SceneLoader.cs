using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour {

	// Use this for initialization
	public void loadscene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }
}

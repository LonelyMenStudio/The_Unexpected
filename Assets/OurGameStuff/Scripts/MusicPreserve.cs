using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPreserve : MonoBehaviour
{
	public int sceneNumber;
	public GameObject[] instances;

	// Use this for initialization
	void Start ()
	{
		instances = GameObject.FindGameObjectsWithTag ("MenuMusic");
		if (instances.Length > 1) {
			Destroy (this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		sceneNumber = SceneManager.GetActiveScene ().buildIndex;
	
		if (sceneNumber > 1) {
			Destroy (this.gameObject);
		}
	}


	void Awake ()
	{
		if (sceneNumber <= 1) {
			DontDestroyOnLoad (this.gameObject);
		}
	}
}
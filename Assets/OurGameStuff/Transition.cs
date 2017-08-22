using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Transition : MonoBehaviour
{
	private AudioSource music;
	public AudioClip lobby;
	public AudioClip calm;
	public AudioClip heat;
	private AudioClip tempStorage;
	public GameObject cM;

	private bool inLobby;

	public float timer = 0;
	public float gameDuration = 0;

	void Start ()
	{
		music = this.gameObject.GetComponent<AudioSource> ();
		tempStorage = lobby;
		inLobby = true;
		music.clip = tempStorage;
		music.Play ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (inLobby && (gameDuration == 0)) {
			gameDuration = cM.GetComponent<GameTimer> ().gameTime;
		}
		timer = cM.GetComponent<GameTimer> ().gameTime;


		if (gameDuration > timer) {
			inLobby = false;
		}
			
			
		if (!inLobby) {

			if (timer < (gameDuration / 2)) {
				tempStorage = heat;
			} else {
				tempStorage = calm;
			}
		} else {
			tempStorage = lobby;
		}
		if (music.clip != tempStorage) {
			music.Stop ();
			music.clip = tempStorage;
			music.Play ();
		}
	}
}

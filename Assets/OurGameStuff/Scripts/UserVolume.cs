using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UserVolume : MonoBehaviour {

	public AudioMixer mixer;
	public Slider masterVol;
	public Text masterText;
	public Slider sfxVol;
	public Text sfxText;
	public Slider musicVol;
	public Text musicText;
	// Use this for initialization
	void Start () {
		float value;
		bool yis;

		yis = mixer.GetFloat ("master", out value);
		if (yis) {	
		masterVol.value = value;
		} else {
			masterVol.value = 0f;
		}
		yis = mixer.GetFloat ("sfx", out value);
		if (yis) {
			sfxVol.value = value;
		} else {
			sfxVol.value = 0f;
		}
		yis = mixer.GetFloat ("music", out value);
		if (yis) {
			musicVol.value = value;
		} else {
			musicVol.value = 0f;
		}
		mixer.SetFloat ("ambient", sfxVol.value);
	}
	
	// Update is called once per frame
	void Update () {
		mixer.SetFloat ("sfx", sfxVol.value);
		mixer.SetFloat ("master", masterVol.value);
		mixer.SetFloat ("music", musicVol.value);
		mixer.SetFloat ("ambient", sfxVol.value);
		masterText.text = "Master: " + (int)(masterVol.value + 80);
		sfxText.text = "SFX: " + (int)(sfxVol.value + 80);
		musicText.text = "Music: " + (int)(musicVol.value + 80);
//		masterText.text = "Master: " + (int)((masterVol.value + 80) * 100 / 80);
//		sfxText.text = "SFX: " + (int)((sfxVol.value + 80) * 100 / 80);
//		musicText.text = "Music: " + (int)((musicVol.value + 80) * 100 / 80);
	}
}

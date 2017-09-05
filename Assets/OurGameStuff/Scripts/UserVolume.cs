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
	}
	
	// Update is called once per frame
	void Update () {
		mixer.SetFloat ("sfx", sfxVol.value);
		mixer.SetFloat ("master", masterVol.value);
		mixer.SetFloat ("music", musicVol.value);
		masterText.text = "Master: " + (int)((masterVol.value + 80) * 100 / 80);
		sfxText.text = "SFX: " + (int)((sfxVol.value + 80) * 100 / 80);
		musicText.text = "Music: " + (int)((musicVol.value + 80) * 100 / 80);
	}
}

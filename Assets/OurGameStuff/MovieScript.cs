using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(AudioSource))]

public class MovieScript : MonoBehaviour {


    public MovieTexture movie;
    private AudioSource audio;

	// Use this for initialization
	void Start () {
        GetComponent<RawImage>().texture = movie as MovieTexture;
        audio.clip = movie.audioClip;
        movie.Play();
        audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape) && movie.isPlaying) {
            movie.Stop();
        }
	}
}

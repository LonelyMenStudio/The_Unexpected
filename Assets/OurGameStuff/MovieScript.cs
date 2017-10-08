using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

//[RequireComponent (typeof(AudioSource))]

public class MovieScript : MonoBehaviour {

    /*  public RawImage image;
     // public MovieTexture movie;
     private VideoPlayer videoPlayer;
      public VideoClip Vid;
      private AudioSource audiosource;
      private VideoSource videoSource;
      // Use this for initialization
      void Start () {
          videoPlayer.source = VideoSource.VideoClip;
          videoPlayer.clip = Vid;
      }

      // Update is called once per frame
      void Update () {
          if (Input.GetKey(KeyCode.Escape)) {

          }
      }*/
    public GameObject canvi;
    public VideoPlayer video;
    void Start() {
        video.Play();
        canvi.SetActive(false);
    }
    void Update() {
        if (Input.GetKey(KeyCode.Escape)) {
            canvi.SetActive(true);
            video.Stop();
        }
    }
}

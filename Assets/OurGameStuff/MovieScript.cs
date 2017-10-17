using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

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
    public GameObject esctext;
    public VideoPlayer video;
    private int sceneNumber;
    public bool MovieHasplayed = false;
    public GameObject[] instances;
    void Start() {
        instances = GameObject.FindGameObjectsWithTag("Video");
        if (instances.Length > 1) {
            MovieHasplayed = true;
            esctext.SetActive(false);
        }
        if (!MovieHasplayed) {
            video.Play();
            canvi.SetActive(false);
            esctext.SetActive(true);
        }
    }
    void Update() {
        sceneNumber = SceneManager.GetActiveScene().buildIndex;

        if (sceneNumber == 2) {
            Destroy(this.gameObject);
        }
        if (Input.GetKey(KeyCode.Escape)) {
            MovieHasplayed = true;
            canvi.SetActive(true);
            esctext.SetActive(false);
            video.Stop();
        }
    }
    void Awake() {
        if (sceneNumber != 2) {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}

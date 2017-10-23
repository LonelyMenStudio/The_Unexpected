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
    public GameObject blackscreen;
    private int sceneNumber;
    public GameObject anayltics;
    public bool MovieHasplayed = false;
    public GameObject[] instances;
    void Start() {

        DontDestroyOnLoad(this.gameObject);
        instances = GameObject.FindGameObjectsWithTag("Video");
        if (instances.Length > 1) {
            MovieHasplayed = true;
            esctext.SetActive(false);
            blackscreen.SetActive(false);
            anayltics.SetActive(false);
            Destroy(instances[0].gameObject);
        }
        if (!MovieHasplayed) {
            //video.Play();
            //canvi.SetActive(false);
            //esctext.SetActive(true);

            Invoke("gogogadget", 0.5f);
        }
    }

    private void gogogadget() {
        video.Play();
        canvi.SetActive(false);
        esctext.SetActive(true);
        blackscreen.SetActive(false);
        //  StartCoroutine(Delay());
        Invoke("delay", 60.0f);
    }
    private void delay() {
        MovieHasplayed = true;
        canvi.SetActive(true);
        esctext.SetActive(false);
        video.Stop();
    }
    void Update() {
        
            sceneNumber = SceneManager.GetActiveScene().buildIndex;
        if (sceneNumber != 2) {
            /* if (sceneNumber == 2) {
                 Destroy(this.gameObject);
             }*/
            if (Input.GetKey(KeyCode.Escape) && sceneNumber == 0) {
                MovieHasplayed = true;
                canvi.SetActive(true);
                esctext.SetActive(false);
                video.Stop();
            }
        }
    }
}


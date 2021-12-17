using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [Header("Text")]
    public GameObject[] infoTexts;
    
    [Header("Audio")] 
    public AudioSource soundPlayer;
    public AudioClip pauseSound;
    public AudioClip playSound;
    private AudioSource audioSource;
    
    [Header("Video")]
    public Material videoMat;
    public GameObject play;
    public GameObject pause;
    private VideoPlayer vp;
    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        videoMat.color = Color.black;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (vp.isPlaying)
            {
                vp.Pause(); //play /pause icons
                audioSource.Pause();
                videoMat.color = new Color32(80,80,80,100) ; //Not fully white
                play.SetActive(false);
                pause.SetActive(true);
                soundPlayer.PlayOneShot(pauseSound);
                infoTexts[1].SetActive(true);
                infoTexts[0].SetActive(false);
            }
                
            else
            {
                pause.SetActive(false);
                play.SetActive(true);
                vp.Play();
                audioSource.Play();
                soundPlayer.PlayOneShot(playSound);
                videoMat.color = Color.black;
                infoTexts[1].SetActive(false);
                infoTexts[0].SetActive(true);
            }
                
        }
    }
}

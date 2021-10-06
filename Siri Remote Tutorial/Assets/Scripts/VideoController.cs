using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
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
                videoMat.color = Color.white;
                play.SetActive(false);
                pause.SetActive(true);
                soundPlayer.PlayOneShot(pauseSound);
            }
                
            else
            {
                pause.SetActive(false);
                play.SetActive(true);
                vp.Play();
                audioSource.Play();
                soundPlayer.PlayOneShot(playSound);
                videoMat.color = Color.black;
            }
                
        }
    }
}

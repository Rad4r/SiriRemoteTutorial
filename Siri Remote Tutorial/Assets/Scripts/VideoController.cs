using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private VideoPlayer vp;
    private AudioSource audioSource;

    public Material videoMat;

    public GameObject play;

    public GameObject pause;
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
            }
                
            else
            {
                pause.SetActive(false);
                play.SetActive(true);
                vp.Play();
                audioSource.Play();
                videoMat.color = Color.black;
            }
                
        }
    }
}

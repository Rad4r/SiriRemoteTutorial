using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Remote Animation")] [SerializeField]
    private Animator remoteAnim;
    
    [Header("Sound Effects")] 
    public AudioSource musicPlayer;
    public AudioClip buttonPress;
    public AudioClip transitionSound;
    private AudioSource soundPlayer;
    
    [Header("Timer")]
    public TextMeshProUGUI timer;
    public bool timerSet;

    [Header("Backgrounds")] public SpriteRenderer[] bg;
    [SerializeField] private int currentBackground;
    
    [Header("Screen Change")] 
    public GameObject[] screens;
    public GameObject continueRemote;
    [SerializeField] private int currentScreen;
    
    private static float MAXTIME;
    private float currentTime;
    private bool screenSevenComplete;
    
    void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
        MAXTIME = 10;
        currentTime = MAXTIME;
        currentBackground = -1;
    }

    void Update()
    {
        if (timerSet)
            TimerUpdate();
        SwitchScreens(); //Could remove from here
        //UpdateBackgroundColor();
    }

    void SwitchScreens()
    {
        switch (currentScreen)
        {
            case 0:
                ChangeScreen();
                UpdateScreenZero();
                UnityEngine.tvOS.Remote.allowExitToHome = true;
                break;
            case 1:
                ChangeScreen();
                UpdateScreenOne();
                remoteAnim.SetBool("Screen1On", true);
                break;
            case 2:
                ChangeScreen();
                UpdateScreenTwo();
                break;
            case 3:
                musicPlayer.Stop();
                ChangeScreen();
                UpdateScreenThree();
                break;
            case 4:
                ChangeScreen();
                UpdateScreenFour();
                remoteAnim.SetBool("Screen4On", true);
                break;
            case 5:
                ChangeScreen();
                UpdateScreenFive();
                remoteAnim.SetBool("Screen5On", true);
                break;
            case 6:
                ChangeScreen();
                UpdateScreenSix();
                remoteAnim.SetBool("Screen6On", true);
                break;
            case 7:
                UnityEngine.tvOS.Remote.allowExitToHome = false;
                screens[7].SetActive(true);
                UpdateScreenSeven();
                break;
            case 8:
                ChangeScreen();
                UpdateScreenThree();
                remoteAnim.SetBool("Screen8On", true);
                break;
            case 9:
                ChangeScreen();
                UpdateScreenThree();
                remoteAnim.SetBool("Screen9On", true);
                break;
            case 10:
                ChangeScreen();
                UpdateScreenLast();
                remoteAnim.SetBool("ScreenLastOn", true);
                break;
            
            default: break;
        }
    }
    
    void UpdateScreenLast()
    {
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            soundPlayer.PlayOneShot(transitionSound);
        }
    }
    void UpdateScreenSeven()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            screenSevenComplete = true;
            currentScreen = 6;
            soundPlayer.PlayOneShot(transitionSound);
            screens[7].GetComponent<Animator>().enabled = false;
            screens[7].SetActive(false);
            screens[6].SetActive(true);
            //Set task done here for transition six
        }
        
        
    }
    void UpdateScreenSix()
    {
        Pointer p = FindObjectOfType<Pointer>();
        if (screenSevenComplete && p.jigsawCompleted)
        {
            p.touchIcon.SetBool("taskDone", true);
            p.infoText[2].GetComponent<TextMeshProUGUI>().text = "Druk op het <color=#4BC8FF><b>aanraakoppervlak</b></color> om verder te gaan";
            if (Input.GetButtonDown("Submit"))
            {
                continueRemote.gameObject.SetActive(false);
                currentScreen=8;
                screens[6].SetActive(false);
                screens[8].SetActive(true);
                soundPlayer.PlayOneShot(transitionSound);
            }
        }
        
    }

    public void OpenSettingsScreen()
    {
        continueRemote.gameObject.SetActive(false);
        currentScreen = 7;
        soundPlayer.PlayOneShot(transitionSound);
    }
    
    void UpdateScreenFour()
    {
        RemotePointer p = FindObjectOfType<RemotePointer>();
        if (p.taskDone && Input.GetButtonDown("Submit"))
        {
            continueRemote.gameObject.SetActive(false);
            currentScreen++;
            soundPlayer.PlayOneShot(transitionSound);
        }
    }
    
    void UpdateScreenTwo()
    {
        if (!timerSet)
        {
            if (Input.GetButtonDown("Submit"))
            {
                continueRemote.gameObject.SetActive(false);
                continueRemote.transform.position += Vector3.up *5f;
                //timer.gameObject.SetActive(true);
                timerSet = true;
                currentScreen++;
                soundPlayer.PlayOneShot(transitionSound);
            }
        }
    }

    void UpdateScreenOne()
    {
        //delay with timer

        if (!timerSet)
        {
            if (Input.GetButtonDown("Submit"))
            {
                continueRemote.gameObject.SetActive(false);
                timer.gameObject.SetActive(true);
                timerSet = true;
                currentScreen++;
                soundPlayer.PlayOneShot(transitionSound);
            }
        }
    }
    void UpdateScreenZero()
    {
        if (Input.GetButtonDown("Submit"))
        {
            continueRemote.gameObject.SetActive(false);
            timer.gameObject.SetActive(true);
            timerSet = true;
            currentScreen++;
            soundPlayer.PlayOneShot(transitionSound);
            musicPlayer.Play();
        }
    }
    void UpdateScreenFive()
    {
        PointerTutorial PT = FindObjectOfType<PointerTutorial>();
        
        if (PT.taskDone && Input.GetButtonDown("Submit"))
        {
            continueRemote.gameObject.SetActive(false);
            currentScreen++;
            soundPlayer.PlayOneShot(transitionSound);
        }
    }
    
    void UpdateScreenThree()
    {
        if (!timerSet)
        {
            if (Input.GetButtonDown("Submit"))
            {
                continueRemote.gameObject.SetActive(false);
                currentScreen++;
                soundPlayer.PlayOneShot(transitionSound);
            }
        }
    }

    // void UpdateBackgroundColor()
    // {
    //     if(currentBackground >= 0)
    //         bg[currentBackground].color = Color.Lerp(bg[currentBackground].color, Color.clear, Time.deltaTime);
    //     if(currentBackground >= 1)
    //         bg[currentBackground-1].gameObject.SetActive(false); //could remove
    // }
    
    void ChangeScreen()
    {
        if (currentScreen > 0)
        {
            screens[currentScreen-1].SetActive(false);
            screens[currentScreen].SetActive(true);
        }
            
    }

    void TimerUpdate()
    {
        if (currentTime <= 1)
            TimerReset();
        else
            currentTime -= Time.deltaTime;
        timer.text = Mathf.Floor(currentTime) + "";
    }

    void TimerReset()
    {
        continueRemote.gameObject.SetActive(true);
        timerSet = false;
        timer.gameObject.SetActive(false);
        currentTime = MAXTIME;
    }
    
    void OnApplicationFocus(bool pauseStatus) {
        if(pauseStatus && currentScreen == 8)
        {
            continueRemote.transform.position += Vector3.down*5f;
            continueRemote.SetActive(true);
        }
    }
}

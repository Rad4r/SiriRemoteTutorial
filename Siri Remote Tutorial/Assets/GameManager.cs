using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
        SwitchScreens();
        UpdateBackgroundColor();
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
                break;
            case 2:
                ChangeScreen();
                UpdateScreenTwo();
                break;
            case 3:
                musicPlayer.Stop();
                ChangeScreen();
                UpdateScreenDefault();
                break;
            case 4:
                ChangeScreen();
                UpdateScreenFour();
                break;
            case 5:
                ChangeScreen();
                UpdateScreenDefault();
                break;
            case 6:
                ChangeScreen();
                UpdateScreenSix();
                break;
            case 7:
                UnityEngine.tvOS.Remote.allowExitToHome = false;
                ChangeScreen();
                UpdateScreenSeven();
                break;
            case 8:
                ChangeScreen();
                UpdateScreenLast();
                break;
            
            default: break;
        }
    }
    
    void UpdateScreenLast()
    {
        if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            soundPlayer.PlayOneShot(transitionSound);
        }
    }
    void UpdateScreenSeven()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            soundPlayer.PlayOneShot(transitionSound);
        }
    }
    void UpdateScreenSix()
    {
        Pointer p = FindObjectOfType<Pointer>();
        if (p.taskDone && Input.GetButtonDown("Submit"))
        {
            continueRemote.gameObject.SetActive(false);
            currentScreen++;
            soundPlayer.PlayOneShot(transitionSound);
        }
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

    void UpdateScreenDefault()
    {
        if (Input.GetButtonDown("Submit"))
        {
            continueRemote.gameObject.SetActive(false);
            currentScreen++;
            soundPlayer.PlayOneShot(transitionSound);
        }
    }

    void UpdateBackgroundColor()
    {
        if(currentBackground >= 0)
            bg[currentBackground].color = Color.Lerp(bg[currentBackground].color, Color.clear, Time.deltaTime);
        if(currentBackground >= 1)
            bg[currentBackground-1].gameObject.SetActive(false); //could remove
    }
    
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
        if (currentTime <= 0)
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
        if(pauseStatus && currentScreen == 6)
        {
            continueRemote.SetActive(true);
        }
    }
}

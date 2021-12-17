using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Remote Animation")] [SerializeField]
    private Animator remoteAnim;
    
    [Header("Sound Effects")] 
    public AudioSource musicPlayer;
    public AudioClip transitionSound;
    private AudioSource soundPlayer;
    
    [Header("Timer")]
    public TextMeshProUGUI timer;
    public bool timerSet;

    [Header("Screen Change")] [SerializeField]
    private Transform screenParent;
    private List<GameObject> screens;
    public GameObject continueRemote;
    [SerializeField] private int currentScreen;
    
    private static float MAXTIME = 10;
    private float currentTime;
    private bool screenSevenComplete;
    
    void Start()
    {
        screens = new List<GameObject>();
        foreach (Transform child in screenParent)
            screens.Add(child.gameObject);
        
        soundPlayer = GetComponent<AudioSource>(); //Could be in a different script
    }

    void Update()
    {
        if (timerSet)
            TimerUpdate();
        SwitchScreens();
    }

    void SwitchScreens()
    {
        if(currentScreen > 0 && currentScreen != 7)
            NextScreen();
        
        switch (currentScreen)
        {
            case 0:
                UnityEngine.tvOS.Remote.allowExitToHome = true;
                if (Input.GetButtonDown("Submit"))
                {
                    UpdateScreenTimer(true,MAXTIME);
                    musicPlayer.Play();
                }
                break;
            case 1:
                remoteAnim.SetBool("Screen1On", true);
                if (!timerSet && Input.GetButtonDown("Submit"))
                    UpdateScreenTimer(true,15);
                break;
            case 2:
                if (!timerSet && Input.GetButtonDown("Submit"))
                {
                    continueRemote.transform.position += Vector3.up *5f;
                    UpdateScreenTimer(false, 15);
                }
                break;
            case 3:
                musicPlayer.Stop();
                if (!timerSet && Input.GetButtonDown("Submit"))
                    UpdateScreen();
                break;
            case 4:
                RemotePointer p = FindObjectOfType<RemotePointer>();
                remoteAnim.SetBool("Screen4On", true);
                if (p.taskDone && Input.GetButtonDown("Submit"))
                    UpdateScreen();
                break;
            case 5:
                PointerTutorial PT = FindObjectOfType<PointerTutorial>();
                remoteAnim.SetBool("Screen5On", true);
                if (PT.taskDone && Input.GetButtonDown("Submit")) 
                    UpdateScreen();
                break;
            case 6:
                UpdateScreenSix();
                remoteAnim.SetBool("Screen6On", true);
                break;
            case 7:
                UnityEngine.tvOS.Remote.allowExitToHome = false;
                screens[7].SetActive(true);
                UpdateScreenSeven();
                break;
            case 8:
                remoteAnim.SetBool("Screen8On", true);
                if (!timerSet && Input.GetButtonDown("Submit"))
                    UpdateScreenTimer(false, 20);
                break;
            case 9:
                remoteAnim.SetBool("Screen9On", true);
                if (!timerSet && Input.GetButtonDown("Submit"))
                    UpdateScreen();
                break;
            case 10:
                remoteAnim.SetBool("ScreenLastOn", true);
                if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel"))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    //soundPlayer.PlayOneShot(transitionSound);
                break;
        }
    }

    void UpdateScreenTimer(bool isTimerVisible, float startTime)
    {
        timer.gameObject.SetActive(isTimerVisible);
        currentTime = startTime;
        timerSet = true;
        UpdateScreen();
    }

    void UpdateScreen()
    {
        continueRemote.gameObject.SetActive(false);
        currentScreen++;
        soundPlayer.PlayOneShot(transitionSound);
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
                
                //new to screen 8
                continueRemote.transform.position -= Vector3.up *3.7f;
                timer.gameObject.SetActive(true);
                timerSet = true;
                currentTime = 15f;
            }
        }
        
    }

    /// <summary>
    /// New stuff
    /// </summary>

    public void OpenSettingsScreen()
    {
        continueRemote.gameObject.SetActive(false);
        currentScreen = 7;
        soundPlayer.PlayOneShot(transitionSound);
    }
    
    void NextScreen()
    {
        screens[currentScreen-1].SetActive(false);
        screens[currentScreen].SetActive(true);
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
    
    void OnApplicationFocus(bool pauseStatus)
    {
        if (screens != null && pauseStatus && currentScreen == screens.Count - 1)
            continueRemote.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Timer")]
    public TextMeshProUGUI timer;
    public bool timerSet;

    [Header("Backgrounds")] public SpriteRenderer[] bg;
    [SerializeField] private int currentBackground;
    
    [Header("ScreenChange")] 
    public GameObject[] screens;
    public GameObject continueRemote;
    [SerializeField] private int currentScreen;
    
    private static float MAXTIME;
    private float currentTime;
    
    void Start()
    {
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
                UpdateScreenZero();
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
                ChangeScreen();
                UpdateScreenDefault();
                break;
            case 4:
                ChangeScreen();
                UpdateScreenDefault();
                break;
            case 5:
                ChangeScreen();
                UpdateScreenDefault();
                break;
            case 6:
                ChangeScreen();
                UpdateScreenDefault();
                break;
            case 7:
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
        if (Input.GetButtonDown("Cancel"))
        {
            currentScreen = 1;
        }
    }
    void UpdateScreenSeven()
    {
        Pointer p = FindObjectOfType<Pointer>();
        if (p.taskDone && Input.GetButtonDown("Submit"))
        {
            continueRemote.gameObject.SetActive(false);
            currentScreen++;
        }
    }

    void UpdateScreenTwo()
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
            }
        }
    }
    void UpdateScreenOne()
    {
        if (Input.GetButtonDown("Submit"))
        {
            continueRemote.gameObject.SetActive(false);
            timer.gameObject.SetActive(true);
            timerSet = true;
            currentScreen++;
        }
    }

    void UpdateScreenDefault()
    {
        if (Input.GetButtonDown("Submit"))
        {
            continueRemote.gameObject.SetActive(false);
            currentScreen++;
        }
    }

    void UpdateScreenZero()
    {
        if (Input.GetButtonDown("Submit")) //Maybe smooth out the transition
        {
            if (currentBackground < bg.Length - 2)
                currentBackground++;
            else
                currentScreen++;
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
        if (currentScreen == 1)
        {
            screens[0].SetActive(false);
            screens[screens.Length -1].SetActive(false);
            screens[currentScreen].SetActive(true);
        }
        else
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

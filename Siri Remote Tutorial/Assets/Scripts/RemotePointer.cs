using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RemotePointer : MonoBehaviour
{
    [Header("Audio")] 
    public AudioClip cursorSwish;
    public AudioClip clickSound;
    private AudioSource soundPlayer;
    [SerializeField]private float buttonSoundSensitivity;

    [Header("Buttons")] 
    public GameObject[] buttons;
    public GameObject instruction;
    public bool taskDone;
    private int currentNumber;


    [Header("Cursor")] public Transform cursor;
    public GameObject firstButton;
    private bool gridSet;

    private void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
    }

    void Update()
    {
        GameObject currentButton = EventSystem.current.currentSelectedGameObject;

        if (!gridSet && Input.GetAxis("Horizontal") > 0)
        {
            EventSystem.current.SetSelectedGameObject(firstButton);
            soundPlayer.PlayOneShot(cursorSwish);
            gridSet = true;
        }

        //SwishSoundPlay();

        if (currentButton != null)
        {
            Vector3 smoothMove = Vector3.Lerp(cursor.position, currentButton.transform.position, Time.deltaTime * 3f);
            cursor.position = smoothMove;

            if (currentButton.CompareTag("blue") && Input.GetButtonDown("Submit"))
            {
                currentButton.GetComponent<Image>().color = Color.white;
                soundPlayer.PlayOneShot(clickSound);
                currentButton.tag = "normal";
                currentNumber++;
                ChangeCurrentBlue();
            }
        }

    }

    void SwishSoundPlay()
    {
        GameObject currentButton = EventSystem.current.currentSelectedGameObject;

        if (!soundPlayer.isPlaying)
        {
            if (currentButton == buttons[0] && (Input.GetAxis("Horizontal") > buttonSoundSensitivity || Input.GetAxis("Vertical") < -buttonSoundSensitivity))
                soundPlayer.PlayOneShot(cursorSwish);
            else if (currentButton == buttons[1] && (Input.GetAxis("Horizontal") < -buttonSoundSensitivity || Input.GetAxis("Vertical") < -buttonSoundSensitivity))
                soundPlayer.PlayOneShot(cursorSwish);
            else if (currentButton == buttons[2] && (Input.GetAxis("Horizontal") > buttonSoundSensitivity || Input.GetAxis("Vertical") > buttonSoundSensitivity))
                soundPlayer.PlayOneShot(cursorSwish);
            else if (currentButton == buttons[3] && (Input.GetAxis("Horizontal") < -buttonSoundSensitivity || Input.GetAxis("Vertical") < -buttonSoundSensitivity))
                soundPlayer.PlayOneShot(cursorSwish);
        }
        
    }


    void SoundReset()
    {
        
    }

    void ChangeCurrentBlue()
    {
        switch (currentNumber)
        {
            case 1:
                buttons[2].tag = "blue";
                buttons[2].GetComponent<Image>().color = new Color32(37, 144, 235,255);
                break;
            case 2:
                buttons[3].tag = "blue";
                buttons[3].GetComponent<Image>().color = new Color32(37, 144, 235,255);
                break;
            case 3:
                buttons[0].tag = "blue";
                buttons[0].GetComponent<Image>().color = new Color32(37, 144, 235,255);
                break;
            case 4:
                Invoke("TaskCompleted", 0.2f);
                break;
            default: break;
        }
    }
    
    void TaskCompleted()
    {
        taskDone = true;
        instruction.SetActive(true);
    }
}
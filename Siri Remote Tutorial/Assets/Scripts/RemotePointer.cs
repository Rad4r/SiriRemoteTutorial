using UnityEngine;
using UnityEngine.EventSystems;

public class RemotePointer : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip clickSound;
    private AudioSource soundPlayer;

    [Header("Buttons")] 
    public GameObject[] buttons;
    public GameObject[] infoTexts;
    public Animator touchAnim;

    [Header("Cursor")] 
    public Transform checkMark;
    private int currentNumber;
    [HideInInspector]public bool taskDone;
    private bool completed;

    private void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
        EventSystem.current.SetSelectedGameObject(buttons[2]);
    }

    void Update()
    {
        GameObject currentButton = EventSystem.current.currentSelectedGameObject;

        if (completed)
        {
            infoTexts[0].SetActive(false);
            infoTexts[1].SetActive(false);
            infoTexts[2].SetActive(true);
            touchAnim.SetBool("taskDone", true);
        }
        else
        {
            if (currentButton.CompareTag("blue"))
            {
                //checkMark.position = currentButton.transform.position;
                touchAnim.SetBool("taskDone", true);
                infoTexts[0].SetActive(false);
                infoTexts[1].SetActive(true);
                if (Input.GetButtonDown("Submit"))
                {
                    soundPlayer.PlayOneShot(clickSound);
                    currentButton.tag = "normal";
                    currentNumber++;
                    ChangeCurrentBlue();
                }
            }
            else
            {
                infoTexts[1].SetActive(false);
                infoTexts[0].SetActive(true);
                touchAnim.SetBool("taskDone", false);
            }
        }
            
        
    }

    void ChangeCurrentBlue()
    {
        switch (currentNumber)
        {
            case 1:
                buttons[2].tag = "blue";
                checkMark.position = buttons[2].transform.position;
                break;
            case 2:
                buttons[3].tag = "blue";
                checkMark.position = buttons[3].transform.position;
                break;
            case 3:
                buttons[0].tag = "blue";
                checkMark.position = buttons[0].transform.position;
                break;
            case 4:
                Completed();
                break;
        }
    }

    void Completed()
    {
        completed = true;
        checkMark.gameObject.SetActive(false);
        Invoke("TaskCompleted", 0.2f);
    }
    
    void TaskCompleted()
    {
        taskDone = true;
    }
}
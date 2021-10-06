using System;
using UnityEditor.Animations;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [Header("Sound")]
    public AudioClip clickSound;
    public AudioClip jigsawPlace;
    private AudioSource soundPlayer;

    [Header("Jigsaw Section")] 
    public Animator touchIcon;
    public AnimatorController animControl;
    public Sprite highlightJigsaw;
    public Sprite normalJigsaw;
    public GameObject textInstructionOne;
    public GameObject textInstructionTwo;
    private Vector3 startPosition;
    private Vector3 jigsawSolution;

    private bool pieceGrabbed;
    [HideInInspector]public bool taskDone;

    private void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
        jigsawSolution = new Vector2(3.5f, 0.05f); // move in two dimensions instead
    }

    void Update()
    {
        PositionClamp();
        TouchMove();
        //NormalMove();
        PositionCheck();
    }

    void NormalMove()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime;
    }

    void PositionCheck()
    {
        Collider2D nearbyObject = Physics2D.OverlapPoint(transform.position);

        if (nearbyObject != null && nearbyObject.CompareTag("jigsaw") && Input.GetButtonDown("Submit"))
        {
            nearbyObject.GetComponent<SpriteRenderer>().sprite = highlightJigsaw;
            pieceGrabbed = true;
            textInstructionOne.SetActive(true);
            soundPlayer.PlayOneShot(clickSound);
        }
        
        if (pieceGrabbed)
        {
            nearbyObject.transform.position = transform.position;
            if (Vector2.Distance(nearbyObject.transform.position, jigsawSolution) <= 0.2f)
            {
                textInstructionTwo.SetActive(true);
                if (Input.GetButtonDown("Submit"))
                {
                    nearbyObject.transform.position = jigsawSolution;
                    nearbyObject.tag = "Untagged";
                    nearbyObject.GetComponent<SpriteRenderer>().sprite = normalJigsaw;
                    pieceGrabbed = false;
                    soundPlayer.PlayOneShot(jigsawPlace);
                    Invoke("TaskComplete", 0.2f);
                    // touchIcon.runtimeAnimatorController = animControl;
                    // taskDone = true;
                }
            }
            else
                textInstructionTwo.SetActive(false);
        }
    }

    void TaskComplete()
    {
        touchIcon.runtimeAnimatorController = animControl;
        taskDone = true;
    }
    
    private void TouchMove()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
                startPosition = transform.position;
            if (touch.phase == TouchPhase.Moved)
                transform.position = new Vector3(touchPosition.x, touchPosition.y,0) + startPosition;
        }
        
    }

    private void PositionClamp()
    {
        Vector2 WorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        Vector2 clampedPosition = new Vector2(Mathf.Clamp(transform.position.x, -WorldPosition.x, WorldPosition.x),
            Mathf.Clamp(transform.position.y, -WorldPosition.y, WorldPosition.y));
        transform.position = clampedPosition;
    }
}

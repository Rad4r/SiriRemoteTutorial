using System;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite normalCursorSprite;
    public Sprite selectedCursorSprite;
    public GameObject highlighter;
    
    [Header("Sound")]
    public AudioClip clickSound;
    public AudioClip jigsawPlace;
    private AudioSource soundPlayer;

    [Header("Jigsaw Section")] 
    //public Animator settingIconAnim;
    public GameObject highlight;
    public Animator touchIcon;
    public Sprite highlightJigsaw;
    public Sprite normalJigsaw;
    public GameObject[] infoText;
    private Vector3 startPosition;
    private Vector3 jigsawSolution;

    private bool pieceGrabbed;
    [HideInInspector]public bool jigsawCompleted;

    private void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
        jigsawSolution = new Vector2(2.11f,-.75f); // move in two dimensions instead
    }

    void Update()
    {
        PositionClamp();
        TouchMove();
        // NormalMove();
        PositionCheck();
    }

    void NormalMove()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * 10f;
    }

    void PositionCheck()
    {
        Collider2D nearbyObject = Physics2D.OverlapPoint(transform.position);

        // if (jigsawCompleted && Input.GetButtonDown("Submit"))
        // {
        //     FindObjectOfType<GameManager>().OpenSettingsScreen();
        // }
        
        if (nearbyObject)
        {
            if (nearbyObject.CompareTag("jigsaw"))
            {
                if (Input.GetButtonDown("Submit"))
                {
                    nearbyObject.GetComponent<SpriteRenderer>().sprite = highlightJigsaw;
                    pieceGrabbed = true;
                    soundPlayer.PlayOneShot(clickSound);
                    infoText[0].SetActive(false);
                    infoText[1].SetActive(true);
                    GetComponent<Image>().sprite = selectedCursorSprite;
                    highlighter.SetActive(true);
                }
            }
        }
        
        if (pieceGrabbed)
        {
            nearbyObject.transform.position = transform.position;
            if (Vector2.Distance(nearbyObject.transform.position, jigsawSolution) <= 0.2f)
            {
                GetComponent<Image>().sprite = normalCursorSprite;
                infoText[0].SetActive(false);
                infoText[1].SetActive(false);
                infoText[2].SetActive(true);
                touchIcon.SetBool("taskDone", true);
                //settingIconAnim.SetBool("StartBlink", true);
                nearbyObject.transform.position = jigsawSolution;
                nearbyObject.tag = "Untagged";
                nearbyObject.GetComponent<SpriteRenderer>().sprite = normalJigsaw;
                pieceGrabbed = false;
                soundPlayer.PlayOneShot(jigsawPlace);
                highlight.SetActive(false);
                jigsawCompleted = true;
            }
            else
            {
                touchIcon.SetBool("taskDone", false);
            }
                
        }
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

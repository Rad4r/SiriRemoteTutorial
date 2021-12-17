using UnityEngine;

public class PointerTutorial : MonoBehaviour
{
    //Also add sounds
    public GameObject[] tutorialTexts;
    public Animator touchIcon;
    private Vector3 startPosition;
    private int remainingPoints;
    [HideInInspector]public bool taskDone;

    private void Start()
    {
        transform.position = Vector3.zero;
        remainingPoints = 4;
    }

    void Update()
    {
        TouchMove();
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * 10f;
        
        if (remainingPoints > 0)
            PositionCheck();
        else
        {
            tutorialTexts[0].SetActive(false);
            tutorialTexts[1].SetActive(false);
            tutorialTexts[2].SetActive(true);
            touchIcon.SetBool("taskDone", true);
            taskDone = true;
        }
            
    }

    void PositionCheck()
    {
        Collider2D nearbyObjs = Physics2D.OverlapPoint(transform.position);
        if (nearbyObjs && nearbyObjs.CompareTag("touchPoint"))
        {
            tutorialTexts[0].SetActive(false);
            tutorialTexts[1].SetActive(true);
            touchIcon.SetBool("taskDone", true);
            if (Input.GetButton("Submit"))
            {
                GetComponent<AudioSource>().Play();
                nearbyObjs.gameObject.SetActive(false);
                remainingPoints--;
            }
        }
        else
        {
            touchIcon.SetBool("taskDone", false);
            tutorialTexts[1].SetActive(false);
            tutorialTexts[0].SetActive(true);
        }
    }

    void TouchMove()
    {
        PositionClamp();
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
    
    void PositionClamp()
    {
        Vector2 WorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        Vector2 clampedPosition = new Vector2(Mathf.Clamp(transform.position.x, -WorldPosition.x, WorldPosition.x),
            Mathf.Clamp(transform.position.y, -WorldPosition.y, WorldPosition.y));
        transform.position = clampedPosition;
    }

}

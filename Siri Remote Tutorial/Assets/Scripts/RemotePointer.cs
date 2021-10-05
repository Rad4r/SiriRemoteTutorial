using UnityEngine;
using UnityEngine.EventSystems;

public class RemotePointer : MonoBehaviour
{
    public Transform cursor;
    public GameObject firstButton;
    private bool gridSet;
    void Update()
    {
        GameObject currentButton = EventSystem.current.currentSelectedGameObject;
        
        if (!gridSet && Input.GetAxis("Horizontal") > 0)
        {
            EventSystem.current.SetSelectedGameObject(firstButton);
            gridSet = true;
        }

        if (currentButton != null)
        {
            Vector3 smoothMove = Vector3.Lerp(cursor.position,currentButton.transform.position, Time.deltaTime * 3f);
            cursor.position = smoothMove;
        }
        
    }
}

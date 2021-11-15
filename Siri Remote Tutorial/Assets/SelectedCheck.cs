using UnityEngine;
using UnityEngine.EventSystems;
public class SelectedCheck : MonoBehaviour
{
    public AudioSource audioPlayer;
    private bool played;
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if (!played)
            {
                audioPlayer.Play();
                played = true;
            }
        }
        else
        {
            played = false;
        }
    }
}


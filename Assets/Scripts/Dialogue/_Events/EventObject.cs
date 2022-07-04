using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour
{
    private EventManager manager;
    private RoomObject roomObject;

    public EventProfile profile;

    public InteractableCharacter[] eventCharacters;
    public Transform playerRespawnLocus;


    private void Awake()
    {
        roomObject = GetComponent<RoomObject>();
    }

    private void Start()
    {
        manager = EventManager.main;
        if (EventManager.isOpen && manager.currentProfile == profile)
            Invoke("SetUpEvent",0.01f);
    }

    public void SetUpEvent()
    {
        manager.room = this;
        profile = manager.currentProfile;

        roomObject.DeactivateCharacters();
        foreach (InteractableCharacter c in eventCharacters)
        {
            c.gameObject.SetActive(true);
            c.enabled = false;
            if(c.character == Character.Detective && profile.evaIsSus)
            {
                c.GetComponentInChildren<Animator>().SetTrigger("isSus");
            }
        }

        PlayerController.main.position=playerRespawnLocus.position;
        PlayerController.main.Invisible = true;
    }

    public void RespawnPlayer()
    {
        foreach (InteractableCharacter c in eventCharacters)
        {
            if (c.character==Character.Butler)
            {
                PlayerController.main.position=c.transform.position;
                PlayerController.main.Invisible = false;
                c.gameObject.SetActive(false);
                return;
            }
        }
    }
}

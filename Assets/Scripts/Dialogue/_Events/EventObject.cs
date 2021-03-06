using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour
{
    private EventManager manager;
    private RoomObject roomObject;

    public EventProfile profile;

    public InteractableCharacter[] eventCharacters;
    public GameObject EventCamera;
    public bool tyrellSits, evaSits, ednaSits, gertieSits;

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

        EventCamera.SetActive(true);

        roomObject.DeactivateCharacters();
        foreach (InteractableCharacter c in eventCharacters)
        {
            c.gameObject.SetActive(true);
            c.enabled = false;
            Animator anim = c.GetComponentInChildren<Animator>();
            if(c.character != Character.Butler)
            {
                anim.SetBool("Sits", c.character == Character.Tycoon ? tyrellSits :
                    c.character == Character.General ? ednaSits :
                    c.character == Character.Gardener ? gertieSits :
                    c.character == Character.Detective ? evaSits : false);
                if (profile.evaIsSus && c.character == Character.Detective)
                {
                    anim.SetTrigger("isSus");
                }
            }
        }

        PlayerController.main.position=playerRespawnLocus.position;
        PlayerController.main.Invisible = true;
    }
    public void DeactivateCharacters()
    {
        foreach (InteractableCharacter c in eventCharacters)
        {
            c.gameObject.SetActive(false);
        }
    }
    public void RespawnPlayer()
    {
        EventCamera.SetActive(false);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : Interactable
{
    public Character character;
    public override void ClickInteract()
    {
        if (PlayerController.main.grounded && !GameManager.isPaused)
        {
            Dialogue d = character.Profile().GetDialogue();
            DialogueManager.main.Open(this, d);
        }        
    }

    public bool inDialogue
    {
        set
        {
            //Do animator stuff
        }
    }
    public void SetAnimation(CharacterEmotion emotion)
    {
        //Do animator stuff
    }
    public bool isTalking
    {
        set
        {
            //Do animator stuff
        }
    }
}

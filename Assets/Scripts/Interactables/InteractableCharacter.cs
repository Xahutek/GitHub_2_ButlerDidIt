using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : Interactable
{
    public Character character;

    public Animator animator;

    public override void Awake()
    {
        base.Awake();
        animator=GetComponentInChildren<Animator>();
    }
    public override void ClickInteract()
    {
        if (PlayerController.main.grounded && !GameManager.isPaused && !EventManager.eventSoon)
        {
            Dialogue d = character.Profile().GetDialogue();
            DialogueManager.main.Open(this, d);
        }        
    }

    public bool inDialogue
    {
        set
        {
            if (animator)
                animator.SetBool("TalkReady",value);
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
            if (animator)
                animator.SetBool("IsTalking", value);
        }
    }

    public void RefreshLocus()
    {
        CharacterLocus locus = GetComponentInParent<CharacterLocus>();
        if (locus != null)
            locus.RefreshMarker();
    }
}

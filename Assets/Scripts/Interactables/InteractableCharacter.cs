using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : Interactable
{
    public Character character;

    public Animator animator;
    public bool facingRight;

    public CharacterLocus locus;

    public override void Awake()
    {
        base.Awake();
        locus = GetComponentInParent<CharacterLocus>();
        animator=GetComponentInChildren<Animator>();
    }
    public override void ClickInteract()
    {
        if (PlayerController.main.grounded && !GameManager.isPaused && !EventManager.eventSoon&& (locus==null||locus.state!=CharacterState.Sleeping))
        {
            Transform t = transform.GetChild(0);

            Dialogue d = character.Profile().GetDialogue();
            DialogueManager.main.Open(this, d);
            t.localScale
                = new Vector3(Mathf.Abs(t.localScale.x) * (facingRight? 1:-1) * Mathf.Sign(PlayerController.main.position.x - t.position.x), t.localScale.y, t.localScale.z);
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

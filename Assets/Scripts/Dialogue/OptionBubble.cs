using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class OptionBubble : SpeechBubble
{
    [HideInInspector] public Dialogue.Option option;
    protected Collider2D colli;

    private void Start()
    {
        colli = GetComponentInChildren<Collider2D>();
    }

    public void Refresh(Dialogue.Option option, Vector2 Root, float heightStack)
    {
        this.option = option;

        if (option == null)
        {
            Disappear();
            return;
        }
        else
        {
            text.text = option.text.CustomParse();
        }

        Show(Root, heightStack);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) { Answer(); }
    }

    public virtual void Answer()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (colli.OverlapPoint(mousePosition))
        {
            DialogueManager.main.OnPickOption(option);
        }
    }
}

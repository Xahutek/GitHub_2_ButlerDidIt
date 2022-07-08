using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class OptionBubble : SpeechBubble
{
    [HideInInspector] public Dialogue.Option option;
    protected Collider2D colli;
    public Color hoverColor;
    Color originColor;
    Image bgImage;
    Tween colorTween;

    private void Start()
    {
        colli = GetComponentInChildren<Collider2D>();
        originColor = container.GetComponent<Image>().color;
        bgImage = container.GetComponent<Image>();
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

    public void OnStartHover()
    {
        DOTween.Kill(colorTween);
        colorTween = container.GetComponent<Image>().DOColor(hoverColor, 0.2f);
    }

    public void OnEndHover()
    {
        DOTween.Kill(colorTween);
        colorTween = container.GetComponent<Image>().DOColor(originColor, 0.2f);
    }
}

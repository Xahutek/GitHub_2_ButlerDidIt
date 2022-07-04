using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class IntroDialogueManager : DialogueManager
{
    [Header("Intro Elements")]
    public Dialogue IntroDialogue;
    public InteractableCharacter ButlerLocus, LordLocus;
    public Transform CoverText, StartButton;

    protected override void Awake()
    {
        TextParser.main = parser;
        main = this;
    }
    public override void Start()
    {
        GlobalBlackscreen.on = false;
        Invoke("StartDialogue",0.1f);
    }
    public void StartDialogue() => Open(ButlerLocus, LordLocus, IntroDialogue);

    public override void Close()
    {
        if (hardEscape) return;
        Clear();
        isOpen = false;
        main=null;
        EventSystem.main = null;
        CoverText.DOScale(1, 0.5f).SetDelay(0.5f).SetEase(Ease.OutBack);
        StartButton.DOScaleY(1, 0.3f).SetDelay(1.5f).SetEase(Ease.OutSine);
    }

}

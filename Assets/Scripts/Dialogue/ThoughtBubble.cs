using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThoughtBubble : SpeechBubble
{
    protected void Start()
    {
        EventSystem.main.OnGetClue += PopUp;
    }
    public void PopUp(Clue C)
    {
        if (DialogueManager.isOpen || !C.isInventoryClue) return;

        if (popUpRoutine!=null) StopCoroutine(popUpRoutine);
        StartCoroutine(ExecuteScenePopUp(C));
    }

    Coroutine popUpRoutine = null;
    IEnumerator ExecuteScenePopUp(Clue C)
    {
        GameManager.manualPaused = true;
        Dialogue.Line l = new Dialogue.Line(Character.Butler, C.name);
        Refresh(null, l, PlayerController.main.position + Vector2.up * 1.5f, 0, true);

        while (isTyping)
        {
            yield return null;
            if(DialogueManager.isOpen)
            {
                Disappear();
                yield break;
            }
        }

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
                break;

            yield return null;
        }

        GameManager.manualPaused = false;

        Disappear();
    }


    public override void Disappear()
    {
        isOpen = false;

        if (scaleRoutine != null) StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(Scale(1f * DialogueManager.main.speed, false));
        DOTween.Kill(MoveTween);
        MoveTween = transform.DOMove(transform.position + Vector3.left * 5, 0.75f * DialogueManager.main.speed).SetEase(Ease.InBack);

        Root = Vector2.zero;
    }
}

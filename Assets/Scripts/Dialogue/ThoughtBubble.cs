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

            transform.position = PlayerController.main.position + Vector2.up * 1.5f;
        }

        float timer = 0;
        while (timer < 0.5f)
        {
            timer+=Time.deltaTime;

            transform.position = PlayerController.main.position + Vector2.up * 1.5f;
        }

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThoughtBubble : SpeechBubble
{
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Comment : MonoBehaviour
{
    public static Comment main;

    public TMP_Text ButlerCommentText;
    public Transform ButlerCommentWindow;

    public void Awake()
    {
        main = this;
    }

    Tween tween;
    public void ShowComment(string text, float time, float delay=0)
    {
        ShowComment(text,delay);
        HideRoutine = StartCoroutine(TimedHide(time+delay));
    }

    Coroutine HideRoutine = null;
    IEnumerator TimedHide(float time)
    {
        float timer = 0;
        while (timer<=time)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        HideComment();
    }


    public void ShowComment(string text, float delay=0)
    {
        if (HideRoutine != null) StopCoroutine(HideRoutine);

        DOTween.Kill(tween);
        tween = ButlerCommentWindow.DOScaleY(1, 0.2f).SetDelay(delay);
        ButlerCommentText.text = text;
    }
    public void HideComment()
    {
        if (HideRoutine != null) StopCoroutine(HideRoutine);

        DOTween.Kill(tween);
        tween = ButlerCommentWindow.DOScaleY(0, 0.2f);
    }
}

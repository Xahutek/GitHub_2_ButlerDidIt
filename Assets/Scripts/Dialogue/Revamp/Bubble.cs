using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Bubble : MonoBehaviour
{
    public RectTransform container;
    public bool isOpen;
    [HideInInspector]public Vector2 Root;
    public bool animated = true;

    public float height
    {
        get { return container.sizeDelta.y; }
    }

    protected Tween
        MoveTween;

    public void Show(Vector2 Root, float heightStack)
    {
        if(this.Root==Root)
            Move(Root, heightStack);
        else 
            Appear(Root, heightStack);
    }

    public void Appear(Vector2 Root, float heightStack)
    {
        isOpen = true;

        this.Root = Root;

        transform.position = Root + Vector2.up * heightStack;
        if (scaleRoutine != null) StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(Scale(0.3f * DialogueManager.main.speed, true));

        Move(Root, heightStack);
    }
    public void Adjust(float heightStack) => Move(Root,heightStack);
    public void Move(Vector2 Root, float heightStack)
    {
        this.Root = Root;

        Vector2 pos = Root + Vector2.up * heightStack;
        if(animated)
        MoveTween = transform.DOMove(pos, 0.3f*DialogueManager.main.speed);
        else
            transform.position = pos;
    }
    public virtual void Disappear()
    {
        isOpen = false;

        if (scaleRoutine != null) StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(Scale(0.15f*DialogueManager.main.speed,false));

        Root = Vector2.zero;
    }

    protected Coroutine scaleRoutine=null;
    public IEnumerator Scale(float time, bool on)
    {
        float startScale = transform.localScale.y;
        float timer=0, i = 0;
        while (timer<time)
        {
            timer += Time.fixedDeltaTime;
            i = timer / time;

            transform.localScale = new Vector3(1, Mathf.Lerp(startScale, on ? 1 : 0, i), 1);

            yield return new WaitForFixedUpdate();
        }
        transform.localScale = new Vector3(1, on ? 1 : 0, 1);
    }
}

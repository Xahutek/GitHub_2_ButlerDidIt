using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InfoKnot : MonoBehaviour
{
    public GameObject
        Hint, 
        Content;

    [TextArea] public string ButlerComment;
    public InfoKnot[] ConnectedKnots;
    public virtual bool isRevealed
    {
        get
        {
            return false;
        }
    }
    public bool isHinted
    {
        get
        {
            foreach (InfoKnot K in ConnectedKnots)
            {
                if (K.isRevealed)
                    return true;
            }
            return isRevealed;
        }
    }

    public Relevance relevance;
    public enum Relevance
    {
        None, Minor, Major
    }

    public virtual Dialogue.Line Comment
    {
        get
        {
            return new Dialogue.Line(Character.Butler,ButlerComment);
        }
    }

    public void Pick()
    {
        if (isRevealed)
            Mindmap.main.ShowComment(this);
    }

    public virtual void Refresh()
    {
        bool
            revealed = isRevealed,
            hinted = isHinted;

        gameObject.SetActive(hinted||revealed);
        Hint.SetActive(hinted);
        Content.SetActive(revealed);
    }

    protected Tween tween;
    public virtual void ToggleEvent(bool on, float duration, Vector3 mid)
    {
        if (on) Refresh();

        float delay= on?Mathf.Clamp01((transform.position - mid).magnitude / 3):0;
        Debug.Log(delay);

        DOTween.Kill(tween);
        transform.localScale = Vector3.one* (!on ? 1 : 0);
        tween = transform.DOScale((on ? 1 : 0), duration).OnComplete(() => Refresh())
            .SetEase(on ? Ease.OutBack : Ease.OutSine)
            .SetDelay(delay);

    }
}
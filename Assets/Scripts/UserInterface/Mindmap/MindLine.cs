using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class MindLine : MonoBehaviour
{
    //private LineRenderer lineRenderer;
    public Color
        MinorColor,
        MajorColor;

    private RectTransform object1;
    private RectTransform object2;
    private Image image;
    private RectTransform rectTransform;

    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Refresh(InfoKnot A, InfoKnot B)
    {

        object1 = A.gameObject.GetComponent<RectTransform>();
        object2 = B.gameObject.GetComponent<RectTransform>();

        RectTransform aux;
        if (object1.localPosition.x > object2.localPosition.x)
        {
            aux = object1;
            object1 = object2;
            object2 = aux;
        }

        rectTransform.localPosition = (object1.localPosition + object2.localPosition) / 2;
        Vector3 dif = object2.localPosition - object1.localPosition;
        rectTransform.sizeDelta = new Vector3(dif.magnitude, 5);
        rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));

        bool
            isEmpty = A.relevance == InfoKnot.Relevance.None || B.relevance == InfoKnot.Relevance.None,
            isMajor = A.relevance == InfoKnot.Relevance.Major || B.relevance == InfoKnot.Relevance.Major;

        image.color = isMajor ? MajorColor : MinorColor;
    }

    protected Tween tween;float currentAlpha;
    public virtual void ToggleEvent(bool on, float duration, Vector3 mid)
    {
        float delay = on ? Mathf.Clamp01((transform.position - mid).magnitude / 3) : 0;
        Debug.Log(delay);

        DOTween.Kill(tween);
        currentAlpha = (!on ? 1 : 0);
        Refresh();
        tween = DOTween.To(() => currentAlpha, x => currentAlpha = x, (on ? 1 : 0), duration)
            .SetEase(on ? Ease.InSine : Ease.OutSine)
            .SetDelay(delay)
            .OnUpdate(() => Refresh());

    }
    public void Refresh()
    {
        Color cov = image.color;
        cov.a = currentAlpha;
        image.color = cov;
    }

    //private void Awake()
    //{
    //     lineRenderer = GetComponent<LineRenderer>();
    //}

    //public void Refresh(InfoKnot A, InfoKnot B)
    //{
    //    lineRenderer.alignment = LineAlignment.TransformZ;
    //    lineRenderer.widthMultiplier = 5;

    //    bool 
    //        isEmpty = A.relevance == InfoKnot.Relevance.None || B.relevance == InfoKnot.Relevance.None,
    //        isMajor = A.relevance == InfoKnot.Relevance.Major || B.relevance == InfoKnot.Relevance.Major;

    //    lineRenderer.colorGradient = isMajor ? MajorColor : MinorColor;
    //    gameObject.SetActive(!isEmpty);

    //    lineRenderer.useWorldSpace = true;

    //    lineRenderer.positionCount = 2;
    //    lineRenderer.SetPosition(0, A.transform.position);
    //    lineRenderer.SetPosition(1, B.transform.position);

    //    mindLines.Add(new InfoKnot[2] {A,B},this);
    //}
}

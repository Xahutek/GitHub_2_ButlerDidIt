using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LiftButton : MonoBehaviour
{
    [SerializeField] Camera liftCam;
    [SerializeField] LiftInterior interior;
    Collider2D col;
    public int floor;

    public Tween scaleTween;
    bool isOpen;

    public void Toggle(bool on,float time , float delay)
    {
        DOTween.Kill(scaleTween);
        scaleTween = transform.DOScale(on ? 1 : 0, time).SetDelay(delay).SetEase(on? Ease.OutBack: Ease.InSine).OnComplete(()=>Activation());
        isOpen = on;
        if (on) Activation();
    }
    public void Activation()
    {
        gameObject.SetActive(isOpen);
    }

    private void Awake()
    {
        isOpen = false;
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (liftCam != null && Portal.isTravelling&& Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (col.OverlapPoint(mousePos))
            {
                interior.ClickFloorButton(floor);
                DOTween.Kill(scaleTween);
                scaleTween = transform.DOScale(1.2f, 0.25f).SetEase(Ease.OutBack);
            }
        }
    }
}

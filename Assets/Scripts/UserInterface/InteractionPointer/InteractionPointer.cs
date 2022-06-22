using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractionPointer : MonoBehaviour
{
    public CharacterLocus targetLocus;
    Tween popUpTween;

    public Vector2
        XSpacing,
        YSpacing;

    public bool isOpen
    {
        get { return targetLocus != null && !DialogueManager.isOpen; }
    }

    private void Start()
    {
        ResetAnimation();
    }
    public void ResetAnimation()
    {
        DOTween.Kill(popUpTween);
        popUpTween = transform.GetChild(0).DOLocalMoveY(0.1f, 0.4f).SetEase(Ease.OutSine).OnComplete(ResetAnimation);
    }

    float scaleI = 0;
    private void FixedUpdate()
    {
        scaleI = Mathf.Clamp01(scaleI + (isOpen ? 1 : -1) * Time.fixedDeltaTime);
        transform.localScale = Vector3.one * scaleI;
    }
    private void Update()
    {
        if (isOpen)
            transform.position = GetPosition();
    }

    public Vector2 GetPosition()
    {
        Camera cam = Camera.main;

        Vector3
            toPos = targetLocus.transform.GetChild(0).position + Vector3.up * 1.25f,
            fromPos = cam.transform.position;
        Vector3 dir = (toPos - fromPos).normalized;

        Vector3 targetScreenPosition = cam.WorldToScreenPoint(toPos);

        bool isOffScreen =
            targetScreenPosition.x <= 0 || targetScreenPosition.x >= Screen.width ||
            targetScreenPosition.y <= 0 || targetScreenPosition.x >= Screen.height;

        if (isOffScreen)
        {
            Vector3 cappedTargetScreenPosition = targetScreenPosition;
            if (cappedTargetScreenPosition.x <= 0) cappedTargetScreenPosition.x = XSpacing.x;
            if (cappedTargetScreenPosition.x >= Screen.width) cappedTargetScreenPosition.x = Screen.width - XSpacing.y;
            if (cappedTargetScreenPosition.y <= 0) cappedTargetScreenPosition.y = YSpacing.x;
            if (cappedTargetScreenPosition.x >= Screen.width) cappedTargetScreenPosition.x = Screen.height - YSpacing.y;

            Vector3 targetWorldPosition = cam.ScreenToWorldPoint(cappedTargetScreenPosition);

            float angle = Vector2.SignedAngle(Vector2.down,dir);
            transform.rotation = Quaternion.Euler(0,0,angle);

            return targetWorldPosition;
        }
        else
        {
            transform.rotation= Quaternion.Euler(0, 0, 0);

            return toPos;
        }
    }
}

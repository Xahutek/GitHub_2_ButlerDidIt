using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPointer : MonoBehaviour
{
    public CharacterLocus targetLocus;

    public bool isOpen
    {
        get { return targetLocus != null; }
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
            toPos = targetLocus.transform.position + Vector3.up * 1.25f,
            fromPos = cam.transform.position;
        Vector3 dir = (toPos - fromPos).normalized;

        Vector3 targetScreenPosition = cam.WorldToScreenPoint(toPos);

        bool isOffScreen =
            targetScreenPosition.x <= 0 || targetScreenPosition.x >= Screen.width ||
            targetScreenPosition.y <= 0 || targetScreenPosition.x >= Screen.height;

        if (isOffScreen)
        {
            Vector3 cappedTargetScreenPosition = targetScreenPosition;
            if (cappedTargetScreenPosition.x <= 0) cappedTargetScreenPosition.x = 0f;
            if (cappedTargetScreenPosition.x >= Screen.width) cappedTargetScreenPosition.x = Screen.width;
            if (cappedTargetScreenPosition.y <= 0) cappedTargetScreenPosition.y = 0f;
            if (cappedTargetScreenPosition.x >= Screen.width) cappedTargetScreenPosition.x = Screen.height;

            Vector3 targetWorldPosition = cam.ScreenToWorldPoint(cappedTargetScreenPosition);
            return targetWorldPosition;
        }
        return toPos;
    }
}

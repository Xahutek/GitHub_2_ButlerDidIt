using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rescale : MonoBehaviour
{
    RectTransform rectTransform;
    public RectTransform modelRectTransform;
    public float addedHeight;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        rectTransform.sizeDelta = new Vector2(modelRectTransform.sizeDelta.x, modelRectTransform.sizeDelta.y + addedHeight);
    }
}

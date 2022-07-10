using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LickBlackscreen : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer screen;
    [HideInInspector] public float i,r;

    private void Awake()
    {
        screen = GetComponent<SpriteRenderer>();
        i = 0;
    }

    private void Update()
    {
        bool on = PlayerController.isLicking;

        r += Time.deltaTime*(360/20);
        r %= 360f;

        i += (on ? 1f : -1f) * Time.deltaTime * (on ? 0.5f : 1);
        i = Mathf.Clamp01(i);

        transform.rotation= Quaternion.Euler(0,0,r);

        Color color = screen.color;
        color.a = i;
        screen.color = color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftUI : MonoBehaviour
{
    public static bool on;
    public LiftButton[] Buttons;
    bool isOpen;

    private void Awake()
    {
        on = false;
        isOpen = false;
        foreach (LiftButton b in Buttons)
        {
            b.transform.localScale = Vector3.zero;
        }
    }
    private void FixedUpdate()
    {
        if (on && !isOpen) Open();
        else if (!on && isOpen) Close();
    }

    public void Open()
    {
        isOpen = true;
        float delay = 0;
        foreach (LiftButton b in Buttons)
        {
            b.Toggle(true,0.5f,delay);
            delay += 0.5f / Buttons.Length;
        }
    }
    public void Close()
    {
        isOpen = false;
        float delay = 0;
        foreach (LiftButton b in Buttons)
        {
            b.Toggle(false,0.3f, delay);
            delay += 0.3f / Buttons.Length;
        }
    }
}

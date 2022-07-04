using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalBlackscreen : MonoBehaviour
{
    public static bool on;

    [HideInInspector]public Image screen;
    TMP_Text Narrative;
    [HideInInspector]public float i;
    public static float multiplier = 1;
    public static string message;

    private void Awake()
    {
        screen = GetComponent<Image>();
        Narrative = GetComponentInChildren<TMP_Text>();

        GlobalBlackscreen.on = false;

        i = 1;
        Color color = screen.color;
        color.a = i;
        screen.color = color;

        if (Narrative)
        {
            color = Narrative.color;
            color.a = i;
            Narrative.color = color;
        }
    }
    private void Update()
    {
        i += (on ? 1f : -1f) * Time.deltaTime*multiplier;
        i = Mathf.Clamp01(i);

        Color color = screen.color;
        color.a = i;
        screen.color = color;
        screen.raycastTarget = on;

        if (Narrative!=null)
        {
            color = Narrative.color;
            color.a = i;
            Narrative.color = color;

            if (i == 0) message = "";

            Narrative.text = message;
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Text Parser")]
public class TextParser : ScriptableObject
{
    public static TextParser main;

    public EffectEntry[] Effects;
    [System.Serializable]public class EffectEntry
    {
        public string text;
        public Color color=Color.white;
    }
    public string Parse(string text)
    {
        foreach (EffectEntry E in Effects)
        {
            if (text.Contains(E.text))
            {
                string S = $"<color=#{ColorUtility.ToHtmlStringRGB(E.color)}>{E.text}</color>";
                text=text.Replace(E.text, S);
            }
        }
        return text;
    }
}

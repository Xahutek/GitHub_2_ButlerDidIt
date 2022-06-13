using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OptionsDisplay : MonoBehaviour
{
    //public RectTransform container;
    //public Vector2 Locus;

    //public Vector2 pos
    //{
    //    get { return Locus+Vector2.up*1.25f; }
    //}

    //public float scale
    //{
    //    get { return container.sizeDelta.y; }
    //}

    //public OptionBubble[] OptionBubbles;
    //public void Refresh(Dialogue d, Vector2 locus)
    //{
    //    Locus = locus;

    //    Dialogue.Option[] options = d.options;
    //    for (int i = 0; i < OptionBubbles.Length; i++)
    //    {
    //        Dialogue.Option option = i < options.Length ? options[i] : null;
    //        if (option != null && option.trigger != null && option.trigger.KnownTo(Character.Butler)) option = null;
    //        OptionBubbles[i].Refresh(option);
    //    }

    //    transform.position = pos;
    //    transform.localScale = new Vector3(1, 0, 1);

    //    transform.DOScaleY(1, 0.4f);
    //}

    //public void Close()
    //{
    //    foreach (OptionBubble O in OptionBubbles)
    //    {
    //        O.Close();
    //    }
    //}

}

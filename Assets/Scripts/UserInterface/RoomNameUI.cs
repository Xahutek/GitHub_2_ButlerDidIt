using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class RoomNameUI : MonoBehaviour
{
    public TMP_Text text;
    public float animSpeed, stayDuration;
    Tween MoveInTween, MoveOutTween;
    private RectTransform rectTransform;

    private void Start()
    {
        EventSystem.main.OnChangeRoom += OnChangeRoom;
        rectTransform = GetComponent<RectTransform>();

        rectTransform.localPosition += new Vector3(600,0, 0);
    }

    private void OnChangeRoom(Room room, Character character)
    {
        string result = room.ToString().Replace("_"," ");

         switch(result.Substring(0,4))
        {
            case "Lord":
                result = result.Insert(4, "'");
                break;
            case "Gert":
                result = result.Insert(6, "'");
                break;
            default:
                break;
        }
        text.text = result; 

        DOTween.Kill(MoveInTween);
        DOTween.Kill(MoveOutTween);

        MoveInTween = rectTransform.DOAnchorPosX(0, animSpeed).SetDelay(0.2f).SetEase(Ease.OutSine);
        MoveOutTween = rectTransform.DOAnchorPosX(600, animSpeed)
            .SetDelay(0.2f +stayDuration).SetEase(Ease.InSine);
    }
}

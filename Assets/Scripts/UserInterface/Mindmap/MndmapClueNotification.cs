using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MndmapClueNotification : MonoBehaviour
{
    protected void Start()
    {
        EventSystem.main.OnGetClue += PopUp;
    }
    Tween popUpTween;
    public void PopUp(Clue C)
    {
        if (!C.isMindmapClue||GameLoadData.difficulty==Difficulty.Butler) return;

        DOTween.Kill(popUpTween);
        popUpTween = transform.DOScaleX(1, 0.2f).SetEase(Ease.InOutSine).SetLoops(6, LoopType.Yoyo);
        Invoke("HardStop",0.2f*6f);
    }
    public void HardStop()
    {
        DOTween.Kill(popUpTween);
        transform.localScale = new Vector3(0,1,1);
    }
}

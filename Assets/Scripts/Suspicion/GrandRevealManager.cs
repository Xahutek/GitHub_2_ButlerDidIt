using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class GrandRevealManager : MonoBehaviour
{
    SuspicionManager suspicionManager;
    public EndScreenManager endScreenManager;

    bool isOpen;

    public Image finalPicture,finalPictureBackground;

    public Sprite
        TheButlerDidIt,
        TheLordDidIt,
        TheGardenerDidIt,
        TheTycoonDidIt,
        TheGeneralDidIt,
        TheDetectiveDidIt,
        TheImposterDidIt;

    private void Start()
    {
        suspicionManager = SuspicionManager.main;
        isOpen = false;
    }

    public void Reveal()
    {
        isOpen = true;
        GameManager.manualPaused = true;

        int sus;
        Character highest = suspicionManager.GetHighest(out sus);

        Reveal(highest);
    }
    public void Reveal(Character highest)
    {
        Sprite WhoDidIt;

        switch (highest)
        {
            case Character.Lord:
                WhoDidIt = TheLordDidIt;
                break;
            case Character.Detective:
                WhoDidIt = TheDetectiveDidIt;
                break;
            case Character.Tycoon:
                WhoDidIt = TheTycoonDidIt;
                break;
            case Character.General:
                WhoDidIt = TheGeneralDidIt;
                break;
            case Character.Gardener:
                WhoDidIt = TheGardenerDidIt;
                break;
            case Character.Imposter:
                WhoDidIt = TheImposterDidIt;
                break;
            default:
                WhoDidIt = TheButlerDidIt;
                break;
        }
        Reveal(WhoDidIt);
    }
    public void Reveal(Sprite WhoDidIt)
    {
        finalPicture.sprite = WhoDidIt;
        finalPictureBackground.raycastTarget = true;

        currentAlpha = 0;
        DOTween.To(() => currentAlpha, i => currentAlpha = i, 1, 0.5f)
            .OnUpdate(() => Refresh()).SetEase(Ease.InExpo);

        finalPicture.transform.parent.localScale = Vector3.one * 2f;
        finalPicture.transform.parent.DOScale(Vector3.one, 0.5f).SetEase(Ease.InExpo);
        finalPicture.transform.parent.rotation = Quaternion.Euler(0, 0, 45f);
        finalPicture.transform.parent.DORotate(Vector3.zero, 0.5f).SetEase(Ease.InExpo);

        endScreenManager.Open();
    }

    float currentAlpha;
    public void Refresh()
    {
        Color 
            cov = finalPicture.color,
            cob = finalPictureBackground.color;
        cov.a = currentAlpha;
        cob.a = currentAlpha;
        finalPicture.color = cov;
        finalPictureBackground.color = cob;
    }
}

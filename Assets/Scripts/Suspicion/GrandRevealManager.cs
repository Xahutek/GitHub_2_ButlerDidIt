using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class GrandRevealManager : MonoBehaviour
{
    SuspicionManager suspicionManager;

    bool isOpen;

    public Image finalPicture;

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
        finalPicture.raycastTarget = true;

        currentAlpha = 0;
        DOTween.To(() => currentAlpha, i => currentAlpha = i, 1, 0.5f)
            .OnUpdate(() => Refresh()).SetEase(Ease.InExpo);

        finalPicture.transform.localScale = Vector3.one * 2f;
        finalPicture.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InExpo);
        finalPicture.transform.rotation = Quaternion.Euler(0, 0, 45f);
        finalPicture.transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.InExpo);
    }

    float currentAlpha;
    public void Refresh()
    {
        Color cov = finalPicture.color;
        cov.a = currentAlpha;
        finalPicture.color = cov;
    }
}

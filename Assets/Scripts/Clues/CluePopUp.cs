using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CluePopUp : MonoBehaviour
{
    public GameObject ItemModule, circle, ItemImage;
    public ClueDisplay ClueDisplay;
    private PlayerController playerReference;
    public Vector2 bubbleOffset, circleOffset;    
    private Vector2 circleOrigin, circleDest, rightUpCorner = new Vector2(Screen.width, Screen.height);
    private Coroutine circleAnim;
    RectTransform containerImage;
    public RectTransform containerText;
    
    [Range(0.2f, 5f)] public float stayDuration = 1.5f;

    public void Start()
    {

        playerReference = PlayerController.main;

        EventSystem.main.OnGetClue += PopUp;
        transform.localPosition = new Vector3(-1200,350,0);
        containerImage = ItemModule.GetComponentInParent<RectTransform>();
        circleOrigin = new Vector2(100, -11);
    }

    Tween SizeTween, PosTween, CircleTween, CircleZoom;
    public void PopUp(Clue C)
    {
        Refresh(C);

        transform.position = ((Vector3)playerReference.position + (Vector3)bubbleOffset + Camera.main.ScreenToWorldPoint(rightUpCorner)) *0.5f;

        DOTween.Kill(SizeTween);
        DOTween.Kill(PosTween);

        transform.localScale = Vector3.zero;

        SizeTween = transform.DOScale(1, 0.2f).SetDelay(0.2f).SetEase(Ease.OutBack);
                  
        if(circleAnim != null)
        {
            DOTween.Kill(CircleTween);
            StopAllCoroutines(); 
        }
        time = 0;
        circleAnim = StartCoroutine(LoopingCircle());
        circle.GetComponent<RectTransform>().anchoredPosition = circleOrigin;
        circleDest = circle.transform.localPosition;
    }

    float time;
    IEnumerator LoopingCircle()
    {
        yield return new WaitForSeconds(0.2f);

        containerImage.anchoredPosition = new Vector2(0, containerText.rect.height / 10);
        containerText.anchoredPosition = new Vector2(0, containerText.rect.height / 10);
        DOTween.Kill(SizeTween);
        DOTween.Kill(PosTween);
        time += 0.2f;
        circle.GetComponent<RectTransform>().anchoredPosition = circleOrigin;

        circle.transform.localScale = Vector3.zero;
        CircleZoom = circle.transform.DOScale(1, 0.15f).SetEase(Ease.OutBack);
        CircleTween = circle.transform.DOLocalMove(circleDest + circleOffset, 0.5f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(0.3f);
        time += 0.3f;

        if (time < stayDuration)
            StartCoroutine(LoopingCircle());
        else
            PosTween = transform.DOLocalMove(new Vector3(-1200, 350, 0), 0.2f).SetDelay(stayDuration + 0.2f).SetEase(Ease.InSine);
    }

    public void Refresh(Clue clue)
    {
        bool isItem = clue is Item;

        ItemModule.SetActive(isItem);
        if (isItem) { 
            ItemImage.GetComponent<Image>().sprite = clue.picture; 
        }
        ClueDisplay.Refresh(clue);
    }
}

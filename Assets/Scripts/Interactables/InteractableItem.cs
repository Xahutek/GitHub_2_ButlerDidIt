using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class InteractableItem : Interactable
{
    private Vector3 pos;
    private Camera cam;
    public float animSpeed = 0.5f;
    public AudioClip pickupSound;

    public override void Awake()
    {
        base.Awake();
        if (Yield&&Yield.KnownTo(Character.Butler))
        {
            gameObject.SetActive(false);
        }
       GetComponent<Image>().sprite = Yield.picture;        
    }

    public override void Availability()
    {
        gameObject.SetActive(!deactivated && Clock.Hour > AvailableTime.x && Clock.Hour < AvailableTime.y
                && (!deactivateIfKnown || !Yield || (Yield && !Yield.KnownTo(Character.Butler))));
    }

    public override void ClickInteract()
    {
        GetComponentInParent<Canvas>().sortingLayerID = SortingLayer.NameToID("UI");

        SoundManager.main.PlayOneShot(pickupSound);
        cam = FindObjectOfType<Camera>();
        pos = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight / 2, 0));

        deactivated = false;
        transform.DOMove(pos, animSpeed).SetEase(Ease.InOutCirc).OnComplete(() => Deactivate()) ;
    }
}

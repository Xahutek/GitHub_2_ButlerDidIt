using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class InteractableItem : Interactable
{
    private Vector3 pos;
    private Camera cam;
    public Item item; 
    public float animSpeed = 0.5f;
    public AudioClip pickupSound;

    private void Awake()
    {
       GetComponent<Image>().sprite = item.picture;        
    }

    public override void ClickInteract()
    {
        GetComponentInParent<Canvas>().sortingLayerID = SortingLayer.NameToID("UI");

        SoundManager.main.effectSource.PlayOneShot(pickupSound);
        cam = FindObjectOfType<Camera>();
        pos = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight / 2, 0));

        item.MakeKnownTo(Character.Butler);
        transform.DOMove(pos, animSpeed).SetEase(Ease.InOutCirc);

        deactivated = true;
        //This doesn't look as nice if the player moves to the left while animation is running
        //as the target pos isn't updating
    }
}

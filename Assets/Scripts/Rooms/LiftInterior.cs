using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LiftInterior : MonoBehaviour
{
    public LiftPortal Second, Ground, Basement;

    public SpriteRenderer[] Lights;

    public static LiftPortal Locus;

    public Camera
        main, fake;

    public Transform dummy, pointer, grid;
    private Vector3 originRot;
    private float addangle = 75;
    private Tween rotTween, squashTween;
    private int currentLevel;
    private bool travelState;
        
    private void Start()
    {
        Locus = null;
        originRot = pointer.eulerAngles;
    }

    public void ClickFloorButton(int floor)
    {
        if(Portal.isTravelling)
            Locus = floor == 0 ? Ground : (floor == 1 ? Second : Basement);
    }

    private void Update()
    {
        if(LiftPortal.liftlocation != null)
        {
            if (currentLevel != LiftPortal.liftlocation.floor)
            {
                currentLevel = LiftPortal.liftlocation.floor;
                DOTween.Kill(rotTween);
                rotTween = pointer.DORotate(originRot + Vector3.forward * -(currentLevel * addangle), 0.3f).SetEase(Ease.InCirc);
            }
            if (LiftPortal.isTravelling != travelState)
            {
                DOTween.Kill(squashTween);
                
                squashTween = grid.DOScaleX(LiftPortal.isTravelling ? 0.12f :0.17f, 0.4f).SetEase(Ease.OutBounce);
                MoveChildRecursive(grid, true);                                
            }
            travelState = LiftPortal.isTravelling;
        }
        dummy.position = transform.position + Portal.offset;
        fake.orthographicSize = main.orthographicSize;
               
    }

    private void MoveChildRecursive(Transform obj, bool opens)
    {
        if (null == obj)
            return;

        foreach (Transform child in obj)
        {
            if (null == child)
                continue;
            child.DOLocalMoveX((opens? -0.25f : -1f), 0.4f).SetEase(Ease.OutBounce);
            MoveChildRecursive(child, opens);
        }
    }
}

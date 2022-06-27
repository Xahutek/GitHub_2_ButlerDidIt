using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LiftPortal : Portal
{
    public GameObject OutsideLiftCamera,InsideLiftCamera;

    public LiftInterior lift;
    public static LiftPortal liftlocation;
    public int floor;
    public Transform pointer;
    public Transform grid;
    private float addangle = 75;
    private Vector3 originRot;
    private Tween squashTween;


    public bool isLiftPos
    {
        get { return liftlocation == this; }
    }

    private void Start()
    {
        originRot = pointer.eulerAngles;
        if (floor == 0) { liftlocation = this; }
        pointer.eulerAngles = originRot + Vector3.forward * -(floor * addangle);
    }

    public override void Interact()
    {
        Debug.Log("Enter Lift");
        base.Interact();
    }
   
    protected override IEnumerator TravelRoutine(PlayerController player)
    {
        isTravelling = true;
        GameManager.manualPaused = true;

        DOTween.Kill(squashTween);

        squashTween = grid.DOScaleX(0.12f, 0.4f).SetEase(Ease.OutBounce);
        MoveChildRecursive(grid, true);

        OutsideLiftCamera.SetActive(true);

        if (!isLiftPos)
        {
            liftlocation = this;
        }

        yield return new WaitForSeconds(0.1f);

        InsideLiftCamera.SetActive(true);

        GlobalBlackscreen.multiplier = 4;
        GlobalBlackscreen.on = true;

        LiftUI.on = true;

        LiftInterior.Locus = null;
        Locus = null;
        float t = 0;
        while (Locus==null||t<1/GlobalBlackscreen.multiplier)
        {
            t+=Time.deltaTime;
            yield return null;
            bool cancel = Input.GetKeyDown(KeyCode.Escape);

            Locus = LiftInterior.Locus;

            if (Locus == lift.Basement && 3 > Clock.Hour)
            {
                LiftInterior.Locus = null;
                Locus=null;
                cancel = true;
                Comment.main.ShowComment("Floor Unavailable. It is still to dark down there!", 3f, 0.75f);
            }
            if (cancel)
            {
                GlobalBlackscreen.multiplier = 1.5f;
                GlobalBlackscreen.on = false;

                yield return new WaitForSeconds(0.25f);
                LiftUI.on = false;
                yield return new WaitForSeconds(0.5f);

                CloseGrid();
                InsideLiftCamera.SetActive(false);
                OutsideLiftCamera.SetActive(false);
                isTravelling = false;
                GameManager.manualPaused = false;
                yield break;
            }

        }

        Debug.Log("Travel with Lift");


        OutsideLiftCamera.SetActive(false);
        (Locus as LiftPortal).OutsideLiftCamera.SetActive(true);

        PlayerController.main.position = Locus.transform.position+offset;
        PlayerController.main.RefreshCollider();

        liftlocation = (Locus as LiftPortal);

        GlobalBlackscreen.multiplier = 1.5f;
        GlobalBlackscreen.on = false;

        yield return new WaitForSeconds(0.25f);
        LiftUI.on = false;
        yield return new WaitForSeconds(0.5f);

        CloseGrid();
        (Locus as LiftPortal).CloseGrid();
        InsideLiftCamera.SetActive(false);
        (Locus as LiftPortal).OutsideLiftCamera.SetActive(false);

        isTravelling = false;
        GameManager.manualPaused = false;
    }

    private void MoveChildRecursive(Transform obj, bool opens)
    {
        if (null == obj)
            return;

        foreach (Transform child in obj)
        {
            if (null == child)
                continue;
            child.DOLocalMoveX((opens ? -0.25f : -1f), 0.4f).SetEase(Ease.OutBounce);
            MoveChildRecursive(child, opens);
        }
    }

    public void CloseGrid()
    {
        grid.localScale = new Vector3(0.12f, grid.localScale.y, grid.localScale.z);
        DOTween.Kill(squashTween);

        squashTween = grid.DOScaleX(0.17f, 0.4f).SetEase(Ease.OutBounce);
        MoveChildRecursive(grid, false);
    }
}

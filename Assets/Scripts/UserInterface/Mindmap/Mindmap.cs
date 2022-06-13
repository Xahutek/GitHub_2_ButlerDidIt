using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;


public class Mindmap : MonoBehaviour
{
    public static Mindmap main;
    public static bool isOpen;

    public MindLine mindLinePrefab;
    public Transform mindlineParent;

    public Image Blackground;
    public float fadeDuration;
    public DraggableImage MindmapObject;

    public InfoKnot[] Knots;

    Tween tween;


    private void Awake()
    {
        main = this;
    }
    private void Start()
    {
        isOpen = false;
        MindmapObject.gameObject.SetActive(false);
    }

    public void ShowComment(InfoKnot info)=>Comment.main.ShowComment(info.Comment.text);
    public void HideComment() => Comment.main.HideComment();

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab) || (isOpen && Input.GetKeyUp(KeyCode.Escape)))
            Toggle();
    }

    public void Toggle()
    {
        if (isOpen)
            Close();
        else Open();
    }

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;
        MindmapObject.gameObject.SetActive(false);

        DOTween.Kill(tween);

        currentAlpha = 0;
        tween = DOTween.To(() => currentAlpha, x => currentAlpha = x, 1, fadeDuration)
            .OnUpdate(() => RefreshBlackground()).OnComplete(() => Refresh());

        HideComment();
    }
    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;
        MindmapObject.gameObject.SetActive(false);

        DOTween.Kill(tween);

        currentAlpha = 1;
        tween = DOTween.To(() => currentAlpha, x => currentAlpha = x, 0, fadeDuration)
            .OnUpdate(() => RefreshBlackground());

        HideComment();
    }

    float currentAlpha;
    public void RefreshBlackground()
    {
        Color cov = Blackground.color;
        cov.a = currentAlpha;
        Blackground.color = cov;
    }


    private Dictionary<Connection, MindLine> mindLines = new Dictionary<Connection, MindLine>();
    private class Connection
    {
        InfoKnot
            A,
            B;
        public bool CheckIdentity(InfoKnot X,InfoKnot Y)
        {
            return (X == A && Y == B) || (Y == A && X == B);
        }
        public Connection(InfoKnot A,InfoKnot B)
        {
            this.A = A;
            this.B = B;
        }
    }
    public void Clear()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (MindLine L in mindLines.Values)
        {
            list.Add(L.gameObject);
        }
        mindLines = new Dictionary<Connection, MindLine>();
        for (int i = 0; i < list.Count; i++)
        {
            GameObject go = list[i];
            list.RemoveAt(i);
            Destroy(go);
            i--;
        }
    }
    public void Refresh()
    {
        MindmapObject.gameObject.SetActive(true);

        foreach (InfoKnot K in Knots)
        {
            K.Refresh();
        }

        Clear();

        foreach (InfoKnot A in Knots)
        {
            if (A == null) continue;

            foreach (InfoKnot B in A.ConnectedKnots)
            {
                if (B == null) continue;

                bool isChecked = false;
                foreach (Connection c in mindLines.Keys)
                {
                    if (c.CheckIdentity(A, B))
                    {
                        isChecked = true;
                        break;
                    }
                }
                if (isChecked) continue;

                bool
                    isEmpty = isEmpty = A.relevance == InfoKnot.Relevance.None || B.relevance == InfoKnot.Relevance.None,
                    isActive = A.isRevealed || B.isRevealed;

                if (!isEmpty && isActive)
                {
                    MindLine L = Instantiate(mindLinePrefab, mindlineParent);
                    L.Refresh(A, B);
                    mindLines.Add(new Connection(A,B),L);
                }
            }
        }
    }


}

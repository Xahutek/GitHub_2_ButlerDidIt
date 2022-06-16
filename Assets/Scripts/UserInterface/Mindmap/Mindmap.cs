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
    public Transform mindlineParent, CenterPopUp;

    public Image Background;
    public float fadeDuration;
    public DraggableImage MindmapObject;

    public InfoKnot[] Knots;

    Tween tween;

    public delegate void MindmapToggleDelegate(bool on,float duration, Vector3 mid);
    public event MindmapToggleDelegate OnMindmapToggle;
    public void ToggleEvent(bool on, float duration)
    {
        OnMindmapToggle?.Invoke(on,duration, CenterPopUp.position);
    }


    private void Awake()
    {
        main = this;
        maxAlpha = Background.color.a;
    }
    private void Start()
    {
        isOpen = false;
        MindmapObject.gameObject.SetActive(false);
        MindmapObject.transform.anchoredPosition = Vector3.zero;
        MindmapObject.transform.localScale = Vector3.one;
        Background.gameObject.SetActive(true);
        currentAlpha = 0;
        RefreshBackground();

        foreach (InfoKnot K in Knots)
        {
            OnMindmapToggle += K.ToggleEvent;
        }
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
        MindmapObject.transform.anchoredPosition = Vector3.zero;
        MindmapObject.transform.localScale = Vector3.one;

        Background.raycastTarget = true;

        DOTween.Kill(tween);
        currentAlpha = 0;
        tween = DOTween.To(() => currentAlpha, x => currentAlpha = x, maxAlpha, fadeDuration)
            .OnUpdate(() => RefreshBackground()).OnComplete(() => Refresh());

        HideComment();
    }
    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;

        Background.raycastTarget = false;

        ToggleEvent(false,fadeDuration);

        DOTween.Kill(tween);
        currentAlpha = maxAlpha;
        tween = DOTween.To(() => currentAlpha, x => currentAlpha = x, 0, fadeDuration)
            .OnUpdate(() => RefreshBackground())
            .OnComplete(() => MindmapObject.gameObject.SetActive(false));

        HideComment();
    }

    float maxAlpha,currentAlpha;
    public void RefreshBackground()
    {
        Color cov = Background.color;
        cov.a = currentAlpha;
        Background.color = cov;
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
            OnMindmapToggle -= L.ToggleEvent;
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
                    OnMindmapToggle += L.ToggleEvent;
                }
            }
        }

        ToggleEvent(true, fadeDuration);
    }


}

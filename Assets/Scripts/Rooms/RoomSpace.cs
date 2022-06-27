using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class RoomSpace : MonoBehaviour
{
    private Collider2D col;
    [SerializeField] private LayerMask LMPlayer;
    [SerializeField] private string SceneName;

    [SerializeField] private SpriteRenderer[] Overlay;
    [SerializeField] private float fadeDuration;
    public bool isOpen, isLoaded;

    //[HideInInspector] public RoomSpace HeadSpace;
    public RoomSpace[] subspaces;
    public bool unloadHandeledByMotherSpace = false;

    private void Awake()
    {
        col=GetComponent<Collider2D>();
        col.enabled = false;
        for (int i = 0; i < Overlay.Length; i++)
        {
            Color cov = Overlay[i].color;
            cov.a = 1;
            Overlay[i].color = cov;
        }
    }

    private void Start()
    {
        Invoke("Enable",0.1f);
        EventSystem.main.OnRefreshRooms += Reload;
    }
    public void Enable()
    {
        col.enabled = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LMPlayer.Contains(collision.gameObject))
        {
            isOpen = true;

            DOTween.Kill(tween);
            Load(true);
            currentAlpha = 1;
            tween = DOTween.To(() => currentAlpha, x => currentAlpha = x, 0, fadeDuration)
                .OnUpdate(() => RefreshOverlay());
        }
    }
    public void ManualFade(bool on)
    {
        DOTween.Kill(tween);

        if (on)
        {
            tween = DOTween.To(() => currentAlpha, x => currentAlpha = x, 0, fadeDuration)
                .OnUpdate(() => RefreshOverlay());
        }
        else
        {
            tween = DOTween.To(() => currentAlpha, x => currentAlpha = x, 1, fadeDuration)
                .OnUpdate(() => RefreshOverlay()).OnComplete(() => Load(false));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LMPlayer.Contains(collision.gameObject))
        {
            isOpen=false;

            DOTween.Kill(tween);
            currentAlpha = 0;
            tween = DOTween.To(() => currentAlpha, x => currentAlpha = x, 1, fadeDuration)
                .OnUpdate(() => RefreshOverlay()).OnComplete(() => Load(false));
        }
    }

    Tween tween;
    float currentAlpha;
    public void RefreshOverlay()
    {
        for (int i = 0; i < Overlay.Length; i++)
        {
            Color cov = Overlay[i].color;
            cov.a = currentAlpha;
            Overlay[i].color = cov;
        }
    }
    public void Reload()
    {
        if (!isOpen) return;

        SceneManager.UnloadSceneAsync(SceneName);
        SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
    }
    public void Load(bool on)
    {
        isLoaded = SceneManager.GetSceneByName(SceneName).isLoaded;
        if(on&&isLoaded&&(isOpen||unloadHandeledByMotherSpace))
        {
            Reload();
        }
        else if (on&&!isLoaded&&(isOpen||unloadHandeledByMotherSpace))
        {
            SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            isLoaded = true;
            GlobalBlackscreen.on = false;
        }
        else if (!on&&isLoaded&&!isOpen&&!unloadHandeledByMotherSpace)
        {
            SceneManager.UnloadSceneAsync(SceneName);
            isLoaded = false;
        }
        Debug.Log(name + ". active " + on);

        foreach (RoomSpace subspace in subspaces)
        {
            Debug.Log("Onto Subspace " + subspace.name + ". active " + on);
            //subspace.HeadSpace = this;
            subspace.currentAlpha = 1;
            subspace.Load(on);
        }

        RefreshOverlay();
    }
}

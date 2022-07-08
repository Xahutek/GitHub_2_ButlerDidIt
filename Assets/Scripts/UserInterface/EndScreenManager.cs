using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EndScreenManager : MonoBehaviour
{
    public Image shade;
    public GlobalBlackscreen localBlackScreen;
    public Transform
        ButtonAgain,
        ButtonQuit;
    public AudioClip paperDrop;

    private void Start()
    {
        currentAlpha = 0;
        Refresh();

        ButtonQuit.localScale = Vector3.one - Vector3.up;
        ButtonAgain.localScale = Vector3.one - Vector3.up;

        localBlackScreen.gameObject.SetActive(false);
    }
    public void Open()
    {
        ButtonAgain.DOScaleY(1, 0.5f).SetDelay(2.5f);
        ButtonQuit.DOScaleY(1, 0.5f).SetDelay(2.5f);

     

        SoundManager.main.PlayOneShot(paperDrop);
        DOTween.To(() => currentAlpha, i => currentAlpha = i, 1, 1f)
            .OnUpdate(() => Refresh()).SetEase(Ease.InOutSine).SetDelay(2.5f);
    }
    public void PlayAgain()
    {
        SaveSystem.SaveNewGame();
        StartCoroutine(ExecuteLoadScene("_MainMenu"));
    }

    float currentAlpha;
    public void Refresh()
    {
        Color cov = shade.color;
        cov.a = currentAlpha;
        shade.color = cov;
    }

    IEnumerator ExecuteLoadScene(string scene)
    {
        localBlackScreen.gameObject.SetActive(true);
        localBlackScreen.i = 0;
        localBlackScreen.screen.color = Color.clear;
        GlobalBlackscreen.on = true;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

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
        ButtonQuit,
        DifficultySelect_Page,
        DifficultySelect_Valet,
        DifficultySelect_Butler;

    private void Start()
    {
        currentAlpha = 0;
        Refresh();

        ButtonQuit.localScale = Vector3.one - Vector3.up;
        ButtonAgain.localScale = Vector3.one - Vector3.up;
        DifficultySelect_Page.localScale = Vector3.one - Vector3.up;
        DifficultySelect_Valet.localScale = Vector3.one - Vector3.up;
        DifficultySelect_Butler.localScale = Vector3.one - Vector3.up;

        localBlackScreen.gameObject.SetActive(false);
    }
    public void Open()
    {
        ButtonAgain.DOScaleY(1, 0.5f).SetDelay(2.5f);
        ButtonQuit.DOScaleY(1, 0.5f).SetDelay(2.5f);

        DifficultySelect_Page.localScale = Vector3.one - Vector3.up;
        DifficultySelect_Valet.localScale = Vector3.one - Vector3.up;
        DifficultySelect_Butler.localScale = Vector3.one - Vector3.up;


        DOTween.To(() => currentAlpha, i => currentAlpha = i, 1, 1f)
            .OnUpdate(() => Refresh()).SetEase(Ease.InOutSine).SetDelay(2.5f);
    }
    public void PlayAgain()
    {
        ButtonAgain.DOScaleY(0, 0.5f);

        DifficultySelect_Page.DOScaleY(1, 0.5f);
        DifficultySelect_Valet.DOScaleY(1, 0.5f);
        DifficultySelect_Butler.DOScaleY(1, 0.5f);
    }

    float currentAlpha;
    public void Refresh()
    {
        Color cov = shade.color;
        cov.a = currentAlpha;
        shade.color = cov;
    }

    public void NewGame(int diff)
    {
        GameLoadData.difficulty = (Difficulty)diff;
        SaveSystem.SaveNewGame();
        LoadGame();
    }
    public void LoadGame()
    {
        StartCoroutine(ExecuteLoadScene("_MainManor"));
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

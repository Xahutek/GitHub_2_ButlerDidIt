using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    Difficulty SelectedDifficulty = Difficulty.Page;
    [SerializeField] GameObject
        PageInfo,
        ValetInfo,
        ButlerInfo;
    [SerializeField] GameObject LoadGameButton;

    private void Start()
    {
        LoadGameButton.SetActive(SaveSystem.GetSavedProgress()!=null);
    }

    public void SelectDifficulty(int diff)
    {
        SelectedDifficulty = (Difficulty)diff;

        PageInfo.SetActive( SelectedDifficulty == Difficulty.Page);
        ValetInfo.SetActive(SelectedDifficulty == Difficulty.Valet);
        ButlerInfo.SetActive(SelectedDifficulty == Difficulty.Butler);
    }
    public void DeseclectDifficulty()
    {
        PageInfo.SetActive(false);
        ValetInfo.SetActive(false);
        ButlerInfo.SetActive(false);
    }


    public void NewGame()
    {
        SaveSystem.SaveNewGame();
        LoadGame();
    }
    public void LoadGame()
    {
        GameLoadData.difficulty = SelectedDifficulty;
        StartCoroutine(ExecuteLoadScene("_MainManor"));
    }

    IEnumerator ExecuteLoadScene(string scene)
    {
        GlobalBlackscreen.on = true;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }
}

public static class GameLoadData
{
    public static Difficulty difficulty;
}
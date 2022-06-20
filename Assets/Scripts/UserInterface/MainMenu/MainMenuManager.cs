using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    Difficulty SelectedDifficulty = Difficulty.Page;
    [SerializeField] GameObject
        PageInfo,
        ValetInfo,
        ButlerInfo;
    [SerializeField] Button LoadGameButton;
    public Image DiffPainting;
    public GameObject unplayedPainting, continuePainting;

    private void Start()
    {
        if (SaveSystem.GetSavedProgress() == null || SaveSystem.GetSavedProgress().Fresh)
        {
            LoadGameButton.enabled = false;
            continuePainting.SetActive(false);
            unplayedPainting.SetActive(true);
        }
        else 
        {
            LoadGameButton.enabled = true;
            unplayedPainting.SetActive(false);
            continuePainting.SetActive(true);
        }
        //SelectedDifficulty = SaveSystem.GetSavedProgress(). //DifficultySetting when available

        DiffPainting.sprite = SelectedDifficulty == Difficulty.Page ? PageInfo.GetComponentInParent<Image>().sprite :
                            SelectedDifficulty == Difficulty.Valet ? ValetInfo.GetComponentInParent<Image>().sprite :
                            ButlerInfo.GetComponentInParent<Image>().sprite;
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
        GameLoadData.difficulty = SelectedDifficulty;
        SaveSystem.SaveNewGame();
        LoadGame();
    }
    public void LoadGame()
    {
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public static bool isOpen;

    public GameObject menu;
    public GameObject OverrideSaveWindow;
    public GameObject ConfirmDelete;
    public GameObject Controls;
    public Clue SaveClue, SaveDeletedClue;

    private void Awake()
    {
        isOpen = false;
        menu.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameManager.isPaused)
                Open();
            else if (isOpen)
                Close();
        }
    }
    public void Open()
    {
        if (isOpen) return;

        isOpen = true;

        GlobalBlackscreen.multiplier = 2;
        GlobalBlackscreen.on = true;

        Invoke("ToggleDelayed", 0.5f);
    }
    public void Close()
    {
        if (!isOpen) return;

        isOpen = false;
        GlobalBlackscreen.multiplier = 2;
        GlobalBlackscreen.on = true;

        Invoke("ToggleDelayed", 0.5f);
        ConfirmDelete.SetActive(false);
        OverrideSaveWindow.SetActive(false);
        Controls.SetActive(false);
    }

    public void ToggleDelayed()
    {
        menu.SetActive(isOpen);

        GlobalBlackscreen.multiplier = 2;
        GlobalBlackscreen.on = false;
    }


    public void SaveAndQuit()
    {
        SaveSystem.SaveProgress();

        Application.Quit();
    }

    public void SaveGame()
    {
        SaveSystem.SaveProgress();
        EventSystem.main.GetClue(SaveClue);
    }
    public void DeleteSave()
    {
        SaveSystem.SaveNewGame();
        EventSystem.main.GetClue(SaveDeletedClue);
    }
    public void OpenWindow(GameObject window)
    {
        window.SetActive(true);
    }
    public void CloseWindow(GameObject window)
    {
        window.SetActive(false);
    }
}

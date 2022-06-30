using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public static bool isOpen;

    public GameObject menu;

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
}

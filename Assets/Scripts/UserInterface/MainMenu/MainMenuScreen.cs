using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour
{
    public static MainScreenType currentScreen=MainScreenType.Main;

    public MainScreenType type;

    public GameObject cam;

    public void SetCurrentScreen(int s)
    {
        currentScreen = (MainScreenType)s;
    }

    private void Awake()
    {
        currentScreen = MainScreenType.Main;
    }
    private void Update()
    {
        cam.SetActive(currentScreen == type);

        if (Input.GetKeyDown(KeyCode.Escape))
            SetCurrentScreen(0);
    }
}
public enum MainScreenType
{
    Main, Options, Credits, Difficulty
}
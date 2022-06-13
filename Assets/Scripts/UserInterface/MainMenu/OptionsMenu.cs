using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject fullScreenCross;

    public void ToggleFullScreen()
    {
        if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            fullScreenCross.SetActive(true);
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            fullScreenCross.SetActive(true);
        }

    }
}

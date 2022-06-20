using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    #region Player Pref Key Constants
    private const string RESOLUTION_PRED_KEY = "resolution";
    #endregion
    public GameObject fullScreenCross;
    List<Resolution> resos = new List<Resolution>();

    public TMP_Text resTxt;
    private int ResIndex = 0;
    [SerializeField] private Slider volumeSlider = null;
    private AudioSource music;

    private void Start()
    {
        music = FindObjectOfType<AudioSource>();
        foreach (Resolution resolution in Screen.resolutions)
            if (resolution.width / resolution.height == 16 / 9)
                resos.Add(resolution);
        ResIndex = PlayerPrefs.GetInt(RESOLUTION_PRED_KEY, 0);
        PlayerPrefs.SetFloat("Master_Volume", 0.5f);
        PlayerPrefs.Save();
    }

    #region FullScreen
    public void ToggleFullScreen()
    {
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            fullScreenCross.SetActive(true);
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            fullScreenCross.SetActive(false);
        }
    }
    #endregion

    #region Resolution
    public void SetResButton(bool increment)
    {
        ResIndex = increment && ResIndex >= resos.Count - 1 ? 0 : 
            !increment && ResIndex <= 0 ? resos.Count - 1 : increment ? ResIndex +1 : ResIndex-1;
        Resolution res = resos[ResIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
        SetResText(res);
    }

    private void SetResText(Resolution res)
    {
        resTxt.text = res.width + " x " + res.height;
    }

    #endregion

    #region Volume
    public void VolumeSlider(float volume)
    {
        PlayerPrefs.SetFloat("Master_Volume", Mathf.Round(volume * 100.0f) * 0.01f);
        PlayerPrefs.Save();
        music.volume = volume;
    }
    #endregion
}

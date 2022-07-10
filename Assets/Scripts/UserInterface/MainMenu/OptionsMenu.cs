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
    [Header("Either of these can be null")]
    public AudioSource music;
    public SoundManager soundManager;

    private void Start()
    {
        foreach (Resolution resolution in Screen.resolutions)
            if (resolution.width / resolution.height == 16 / 9)
                resos.Add(resolution);
        ResIndex = PlayerPrefs.GetInt(RESOLUTION_PRED_KEY, 0);
        SetResText(Screen.width, Screen.height);
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
    private void SetResText(int x, int y)
    {
        resTxt.text = x + " x " + y;
    }

    #endregion

    #region Volume
    public void VolumeSlider(float volume)
    {
        PlayerPrefs.SetFloat("Master_Volume", Mathf.Round(volume * 100.0f) * 0.01f);
        PlayerPrefs.Save();

        if(music) music.volume = volume;
        if (soundManager) soundManager.RefreshVolume();
    }
    #endregion
}

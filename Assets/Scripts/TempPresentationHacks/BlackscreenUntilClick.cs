using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackscreenUntilClick : MonoBehaviour
{
    public GameObject waitGraphic;
    public AudioSource source;
    private void Start()
    {
        GlobalBlackscreen.on = true;
        GameManager.manualPaused = true;
        oneShot = true;
        waitGraphic.SetActive(true);
        source.Stop();
    }
    public bool oneShot;
    private void Update()
    {
        if (oneShot&&Input.GetKeyDown(KeyCode.Space))
        {
            oneShot = false;
            GlobalBlackscreen.on = false;
            GameManager.manualPaused = false;
            waitGraphic.SetActive(false);
            source.Play();
            Clock.Hour = 8.995f;
        }
    }
}

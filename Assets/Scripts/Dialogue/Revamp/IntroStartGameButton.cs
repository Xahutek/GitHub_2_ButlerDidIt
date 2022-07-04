using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class IntroStartGameButton : OptionBubble
{
    bool clicked;
    
    public override void Answer()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!clicked&&colli.OverlapPoint(mousePosition))
        {
            clicked = true;
            GlobalBlackscreen.multiplier = 1;
            GlobalBlackscreen.on = true;
            Invoke("StartGame", 1.25f);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("_MainManor");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DifficultyDisplay : MonoBehaviour
{
    Image display;
    public Sprite
        Page, Valet, Butler;
    private void Awake()
    {
        display = GetComponent<Image>();
    }
    private void FixedUpdate()
    {
        switch (GameLoadData.difficulty)
        {
            case Difficulty.Page: 
                display.sprite = Page;
                break;
            case Difficulty.Valet:
                display.sprite = Valet;
                break;
            case Difficulty.Butler:
                display.sprite = Butler;
                break;
            default:
                break;
        }
    }

}

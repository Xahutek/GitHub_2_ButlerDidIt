using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameObject : MonoBehaviour
{
    public Canvas canvas;

    private void Start()
    {
        if (canvas) 
        {
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = "UI";
            canvas.sortingOrder = 90;
        }
        Open();
    }
    public virtual void Open()
    {
        MinigameManager.currentMinigame = this;
        Load();
    }
    public virtual void Load()
    {
        
    }
    public virtual void Close()
    {
        Save();
    }
    public virtual void Save()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableMinigame : Interactable
{
    public string minigameName;
    public override void ClickInteract()
    {
        if (!SceneManager.GetSceneByName(minigameName).isLoaded)
        {
            MinigameManager.main.Open(minigameName);
        }
    }
}

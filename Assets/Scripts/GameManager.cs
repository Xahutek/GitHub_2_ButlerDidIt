using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isPaused; [SerializeField] private bool _isPaused;
    public static bool manualPaused;

    DialogueManager dialogueManager;
    InventoryUI inventoryUI;
    Mindmap mindmap;
    EventManager eventManager;

    public Clue SaveClue, SaveDeletedClue;

    private void Awake()
    {
        manualPaused = true;
    }
    private void Start()
    {
        dialogueManager = DialogueManager.main;
        inventoryUI = InventoryUI.main;
        mindmap = Mindmap.main;
        eventManager = EventManager.main;

        Invoke("LoadSave",0.1f);
    }

    public void LoadSave()
    {
        SaveSystem.LoadProgress();
        manualPaused = false;
    }

    private void Update()
    {
        isPaused
             = manualPaused
            || PauseUI.isOpen
            || DialogueManager.isOpen
            || InventoryUI.isOpen
            || Mindmap.isOpen
            || EventManager.isOpen
            || MinigameManager.isOpen
            || Portal.isTravelling;
        _isPaused = isPaused;

        #if UNITY_EDITOR

        if (Input.GetKeyUp(KeyCode.Y))
        {
            SaveSystem.SaveProgress();
            EventSystem.main.GetClue(SaveClue);
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            SaveSystem.SaveNewGame();
            EventSystem.main.GetClue(SaveDeletedClue);
        }
        #endif
    }
}

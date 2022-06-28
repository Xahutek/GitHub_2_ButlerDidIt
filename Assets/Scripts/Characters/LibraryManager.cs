using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryManager : MonoBehaviour
{
    public CharacterProfile[] Profiles;
    public ClueLibrary clueLibrary;

    private void Awake()
    {
        foreach (Item i in clueLibrary.AllItems)
        {
            i.isMindmapClue = false;
            i.isInventoryClue = true;
        }
        foreach (Clue c in clueLibrary.AllClues)
        {
            c.isMindmapClue = false;
            c.isInventoryClue = false;
        }
    }
    public void Start()
    {
        clueLibrary.SetUp();
        foreach (CharacterProfile c in Profiles)
        {
            c.SetUp();
        }
    }
}

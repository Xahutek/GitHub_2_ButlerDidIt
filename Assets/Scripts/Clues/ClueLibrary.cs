using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ClueLibrary")]
public class ClueLibrary : ScriptableObject
{
    public static ClueLibrary main;

    public Clue[] AllClues;
    public List<Note> AllNotes= new List<Note>();
    public Item[] AllItems;

    public void SetUp()
    {
        main = this;

        foreach (Clue c in AllClues)
        {
            c.FullReset();
        }
        foreach (Item i in AllItems)
        {
            i.FullReset();
            i.FullGivenReset();
        }
    }
}

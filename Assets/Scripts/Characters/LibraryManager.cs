using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryManager : MonoBehaviour
{
    public CharacterProfile[] Profiles;
    public ClueLibrary clueLibrary;

    public void Start()
    {
        clueLibrary.SetUp();
        foreach (CharacterProfile c in Profiles)
        {
            c.SetUp();
        }
    }
}

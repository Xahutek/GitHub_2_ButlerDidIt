using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharacterInfoKnot : InfoKnot
{
    public CharacterProfile character;
    public override bool isRevealed
    {
        get
        {
            return character != null && character.knownToPlayer;
        }
    }
}
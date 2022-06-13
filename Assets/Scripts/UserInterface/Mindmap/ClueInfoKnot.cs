using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueInfoKnot : InfoKnot
{
    public Clue clue;
    public override bool isRevealed
    {
        get
        {
            return clue == null || clue.KnownTo(Character.Butler);
        }
    }

}

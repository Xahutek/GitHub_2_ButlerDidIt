using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    //LayerMask
    public static bool Contains(this LayerMask LM,GameObject obj)
    {
        return ((LM.value & (1 << obj.layer)) > 0);
    }

    public static CharacterProfile Profile(this Character C)
    {
        CharacterProfile P = null;
        switch (C)
        {
            case Character.Butler:
                P = CharacterProfile.Butler;
                break;
            case Character.Lord:
                P = CharacterProfile.Lord;
                break;
            case Character.Detective:
                P = CharacterProfile.Detective;
                break;
            case Character.Tycoon:
                P = CharacterProfile.Tycoon;
                break;
            case Character.General:
                P = CharacterProfile.General;
                break;
            case Character.Gardener:
                P = CharacterProfile.Gardener;
                break;
            case Character.Imposter:
                P = CharacterProfile.Inposter;
                break;
            default:
                break;
        }
        if (P == null) 
            Debug.LogError("The Requested Character Profile reference does not exist");
        return P;
    }

    public static string CustomParse(this string S)
    {
        return TextParser.main.Parse(S);
    }
}

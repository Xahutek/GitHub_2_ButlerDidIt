using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueGiver : MonoBehaviour
{
    public Clue Yield;
    public Character Character = Character.Detective;
    public float time;

    private void FixedUpdate()
    {
        if (time >= Clock.Hour && !Yield.KnownTo(Character))
        {
            Yield.MakeKnownTo(Character);
            Debug.Log(Character+" found out about "+ Yield.name +" because it is past "+time);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueGiver : MonoBehaviour
{
    public Clue Yield;
    public Character Character = Character.Detective;
    public float time;

    public Clue[] ButlerKnowsCondition, ButlerNotKnowsCondition;

    private void FixedUpdate()
    {
        if (time >= Clock.Hour && !Yield.KnownTo(Character))
        {
            foreach (Clue c in ButlerKnowsCondition)
            {
                if (!c.KnownTo(Character.Butler))
                    return;
            }

            foreach (Clue c in ButlerNotKnowsCondition)
            {
                if (c.KnownTo(Character.Butler))
                    return;
            }

            Yield.MakeKnownTo(Character);
            Debug.Log(Character+" found out about "+ Yield.name +" because it is past "+time);
        }
    }
}

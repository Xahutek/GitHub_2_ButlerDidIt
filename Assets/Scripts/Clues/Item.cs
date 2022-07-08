using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class Item : Clue
{
    public bool givenAway;
    [SerializeField] private bool _givenAway;

    public void FullGivenReset()
    {
        givenAway = _givenAway;
    }

    public override void ApplyAlsoAffected()
    {
        base.ApplyAlsoAffected();
        foreach (Clue c in AlsoAffected)
        {
            if (c &&(c is Item))
                (c as Item).givenAway = givenAway;
        }
    }
}

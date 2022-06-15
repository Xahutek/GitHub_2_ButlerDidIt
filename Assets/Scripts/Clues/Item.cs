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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedObject : MonoBehaviour
{
    [Header("Children are only active in given time")]
    public bool available;

    public bool checkTime = true;
    public Vector2 AvailableHours = new Vector2(0, 24);

    public bool checkClues;
    public Clue[] ButlerKnownClues;
    public Clue[] ButlerUnknownClues;



    private List<GameObject> objects = new List<GameObject>();
    private void Awake()
    {
        available = false;
        foreach (Transform t in transform)
        {
            objects.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        available = true;
        if (checkTime)
        {
            bool checkTime = Clock.Hour > AvailableHours.x && Clock.Hour < AvailableHours.y;
            if (checkTime != available)
            {
                available = checkTime;
            }
        }
        if (checkClues)
        {
            foreach (Clue c in ButlerKnownClues)
            {
                if (!c.KnownTo(Character.Butler)) available = false;
            }
            foreach (Clue c in ButlerUnknownClues)
            {
                if (c.KnownTo(Character.Butler)) available = false;
            }
        }

        Debug.Log(available);
        foreach (GameObject o in objects)
        {
            o.gameObject.SetActive(available);
        }
    }
}

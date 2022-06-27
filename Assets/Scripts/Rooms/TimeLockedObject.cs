using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLockedObject : MonoBehaviour
{
    [Header("Children are only active in given time")]
    public bool available;
    public Vector2 AvailableHours = new Vector2(0, 24);


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
        bool checkTime= Clock.Hour > AvailableHours.x && Clock.Hour < AvailableHours.y;
        if (checkTime!=available)
        {
            available = checkTime;
            Debug.Log(available);
            foreach (GameObject o in objects)
            {
                o.gameObject.SetActive(available);
            }
        }
    }
}

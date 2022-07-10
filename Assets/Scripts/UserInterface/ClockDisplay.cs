using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ClockDisplay : MonoBehaviour
{
    public RectTransform minuteHand, hourHand;

    //just for testing    
    const float hoursToDegrees = 360 / 12, minutesToDegrees = 360 / 60;

    public Item PocketWatchItem;


    void FixedUpdate()
    {
        hourHand.localRotation = Quaternion.Euler(0, 0, -Clock.Hour * hoursToDegrees);
        minuteHand.localRotation = Quaternion.Euler(0, 0, -Clock.Minute * minutesToDegrees);

        UpdateMarkers();

        if (!PocketWatchItem.KnownTo(Character.Butler)||PocketWatchItem.givenAway)
            gameObject.SetActive(false);
    }

    public ClockMarker[] markers= new ClockMarker[0];
    [System.Serializable]public class ClockMarker
    {
        public string name;
        public EventProfile date;
        public Clue clue;
        public ClueInfoKnot knot;
        public Image marker;
    }

    public void UpdateMarkers()
    {
        foreach (ClockMarker marker in markers)
        {
            float
                eventTime = marker.date.availableTime.x,
                currentTime = Clock.Hour,
                intensity = Mathf.Clamp01(1 - ((eventTime - currentTime) / 10));

            bool active =
                (marker.clue==null|| marker.clue.KnownTo(Character.Butler)) &&
                currentTime < eventTime &&
                eventTime - currentTime < 10;

            marker.marker.gameObject.SetActive(active);
            Color c = marker.marker.color;
            c.a = intensity;
            marker.marker.color = c;
        }
    }

    public void PickMarker(int index)
    {
        ClockMarker marker= markers[index];

        Mindmap.main.Open();
        marker.knot.Pick();
    }
}

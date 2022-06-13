using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class TimetableEntry : MonoBehaviour
{
    public CharacterProfile.TimeBox entry;

    public TMP_Text
        TimeText,
        PlaceText;

    public void Refresh(CharacterProfile.TimeBox box)
    {
        entry = box;

        float
            hours = Mathf.FloorToInt(box.startHour),
            minutes = (box.startHour - hours) * 60;

        TimeText.text = string.Format("{0:00}:{1:00}", hours, minutes);

        PlaceText.text = entry.room.ToString();
    }
}

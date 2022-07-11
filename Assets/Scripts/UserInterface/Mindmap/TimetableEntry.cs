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

        PlaceText.text = RoomName(entry.room);
    }
    private string RoomName(Room room)
    {
        string result = room.ToString().Replace("_", " ");

        switch (result.Substring(0, 4))
        {
            case "Lord":
                result = result.Insert(4, "'");
                break;
            case "Gert":
                result = result.Insert(6, "'");
                break;
            default:
                break;
        }
        return result;
    }

    }

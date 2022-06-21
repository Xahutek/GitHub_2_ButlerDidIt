using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CharacterInfoKnot : InfoKnot, IPointerEnterHandler, IPointerExitHandler
{
    public CharacterProfile character;
    public GameObject RoomComment;
    public override bool isRevealed
    {
        get
        {
            return character != null && character.knownToPlayer;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RoomComment.SetActive(true);
        RoomComment.GetComponentInChildren<TMP_Text>().text = character == CharacterProfile.Butler ? "I'm just standing here." 
            : character.CurrentTimeBox.room.ToString() == "Null" ? "Probably asleep currently" 
            : "Current Location: " + CorrectRoomName(character.CurrentTimeBox.room);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RoomComment.SetActive(false);
    }

    private string CorrectRoomName(Room room)
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
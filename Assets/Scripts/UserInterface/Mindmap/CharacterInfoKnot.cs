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
    public GameObject portrait;

    private void Start()
    {
        portrait = Instantiate(character.Portrait, Content.transform);
        portrait.transform.SetAsFirstSibling();
    }
    private void FixedUpdate()
    {
        portrait.GetComponent<RectTransform>().sizeDelta = Content.GetComponent<RectTransform>().sizeDelta;
    }
    public override bool isRevealed
    {
        get
        {
            if(character == Character.Imposter.Profile() && Character.Detective.Profile().knownToPlayer)
            {
                return true;
            }
            return character != null && character.knownToPlayer;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(character != Character.Imposter.Profile() || character == (Character.Imposter.Profile() && !Character.Detective.Profile().knownToPlayer))
        {
            RoomComment.SetActive(true);
            RoomComment.GetComponentInChildren<TMP_Text>().text = character == CharacterProfile.Butler ? "I'm just standing here."
                : character.CurrentTimeBox.room.ToString() == "Null" ? "Probably asleep currently"
                : "Current Location: " + CorrectRoomName(character.CurrentTimeBox.room);
        }
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
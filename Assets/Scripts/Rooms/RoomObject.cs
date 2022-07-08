using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    public Room identity;

    public CharacterLocus[] Loci;

    private void Start()
    {
        EventSystem.main.OnRefreshRooms += RoomRefresh;
        Refresh();
    }
    private void OnDisable()
    {
        EventSystem.main.OnRefreshRooms -= RoomRefresh;
    }
    public void RoomRefresh()
    {
        DeactivateCharacters();
        Refresh();
    }
    public void Refresh()
    {
        Debug.Log("REFRESH "+name);
        foreach (Character c in System.Enum.GetValues(typeof(Character)))
        {
            if (c == Character.Butler || c == Character.Lord || EventManager.blockRoomRefreshs) continue;

            CharacterProfile p = c.Profile();
            CharacterProfile.TimeBox b = p.CurrentTimeBox;
            foreach (CharacterLocus l in Loci)
            {
                l.Refresh(b,identity,c);
            }
        }
    }
    public void DeactivateCharacters()
    {
        foreach (CharacterLocus l in Loci)
        {
            l.active = false;
            l.gameObject.SetActive(false);
        }
    }
}

public enum Room
{
    Null, Driveway, Garden, Gerties_Shack, Atrium, Living_Room, Library, Bathroom, Dining_Hall,
    Lords_Study,Lords_Bedchamber,Lords_Secret_Room,Puppetry, Blocked_Staircase,
    Kitchen,Wine_Cellar, Food_Chamber, Maintenance, Conservatory, Treetop,
    PresentationHallway
}
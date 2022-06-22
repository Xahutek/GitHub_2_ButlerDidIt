using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSounds : MonoBehaviour
{
    public AudioClip[] carpetSteps, woodSteps, stoneSteps, grassSteps;
    AudioClip[] currentList;
    public Room[] carpetRooms, woodRooms, stoneRooms, grassRooms;

    private void Start()
    {
        EventSystem.main.OnChangeRoom += OnChangeRoom;
    }

    private void OnChangeRoom(Room room, Character character)
    {
        currentList = SearchForRoom(carpetRooms, room) ? carpetSteps : 
            SearchForRoom(woodRooms, room) ? woodSteps : 
            SearchForRoom(stoneRooms, room) ? woodSteps : 
            SearchForRoom(grassRooms, room) ? grassSteps : null;
    }

    private bool SearchForRoom(Room[] roomList, Room room)
    {
        bool returnValue = false;
        foreach(Room roomUnit in roomList)
        {
            returnValue = room == roomUnit ? true : false;
            if (returnValue) { return returnValue; }
        }
        return returnValue;
    }
    public void MakeStepNoise() 
    {
        SoundManager.main.PlayOneShot(RandomStep(currentList));
    }
    
    private AudioClip RandomStep(AudioClip[] clipList)
    {        
        return clipList[Random.Range(0, clipList.Length)];
    }
}

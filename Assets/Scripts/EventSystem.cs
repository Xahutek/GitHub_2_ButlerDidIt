using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem main;

    private void OnEnable()
    {
        if(main==null)
        main = this;
    }

    public delegate void ChangeRoomDelegate(Room room, Character character);
    public event ChangeRoomDelegate OnChangeRoom;
    public void ChangeRoom(Room room, Character character)
    {
        Debug.Log(character.ToString()+" changed to " + room.ToString());
        OnChangeRoom?.Invoke(room, character);
    }

    public delegate void InteractDelegate(Interactable I);
    public event InteractDelegate OnInteract;
    public void Interact(Interactable I)
    {
        OnInteract?.Invoke(I);
    }

    public delegate void NoteDelegate(Note N);
    public event NoteDelegate OnNoteDown;
    public void MakeNote(Note N)
    {
        OnNoteDown?.Invoke(N);
    }

    public delegate void ClueDelegate(Clue C);
    public event ClueDelegate OnPickClue, OnGetClue;
    public void PickClue(Clue C)
    {
        OnPickClue?.Invoke(C);
    }
    public void GetClue(Clue C)
    {
        OnGetClue?.Invoke(C);
    }
}

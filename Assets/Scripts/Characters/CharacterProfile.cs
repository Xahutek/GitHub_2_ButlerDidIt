using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterProfile")]
public class CharacterProfile : ScriptableObject
{
    #region StaticMembers

    public static CharacterProfile[] list = new CharacterProfile[7];
    public static CharacterProfile
        Butler,
        Lord,
        Detective,
        Tycoon,
        General,
        Gardener,
        Inposter;

    #endregion

    public Character identity;
    public new string name;
    public Sprite Portrait;
    public Color Color;
    public bool knownToPlayer;
    public Room currentRoom;


    #region Timetable

    public TimeBox[] Timetable = new TimeBox[1];
    [System.Serializable] public class TimeBox
    {
        public float startHour;
        public Room room;
        public CharacterState state;
        public bool important;

        public TimeBox(float t, Room r, CharacterState s)
        {
            startHour = t;
            room = r;
            state = s;
        }
    }
    public TimeBox CurrentTimeBox
    {
        get
        {
            float hour = Mathf.Clamp(Clock.Hour, 0, 24);
            TimeBox Box = Timetable[0];
            foreach (TimeBox B in Timetable)
            {
                if (B.startHour < hour)
                    Box = B;
            }
            return Box;
        }
    }

    #endregion

    #region Clues & Dialogue

    public Clue[]
        connectedClues;

    public Dialogue[]
        dialogues;
    public Dialogue nullDialogue,nullDialogueReaction;

    public Dialogue GetDialogue(Clue input = null)
    {
        List<Dialogue> options = new List<Dialogue>();

        foreach (Dialogue d in dialogues)
        {
            if (d.isValid(input))
                options.Add(d);
        }
        if (options.Count == 0)
            return input ? nullDialogueReaction : nullDialogue;

        return options[0];
    }

    #endregion

    public void SetUp()
    {
        switch (identity)
        {
            case Character.Butler:
                Butler = this;
                break;
            case Character.Lord:
                Lord = this;
                break;
            case Character.Detective:
                Debug.Log("Detective Set up");
                Detective = this;
                break;
            case Character.Tycoon:
                Tycoon = this;
                break;
            case Character.General:
                General = this;
                break;
            case Character.Gardener:
                Gardener = this;
                break;
            case Character.Imposter:
                Inposter = this;
                break;
            default:
                break;
        }
        list[(int)identity] = this;

        foreach (Dialogue d in dialogues)
        {
            d.seen = false;
        }
        foreach (Clue c in connectedClues)
        {
            c.isInventoryClue = true;
        }

        currentRoom = Room.Null;
        bool knowntoplayer = identity == Character.Butler || identity == Character.Lord || identity == Character.Gardener;

        EventSystem events = EventSystem.main;
        events.OnChangeRoom += OnChangeRoom;
    }

    public void OnChangeRoom(Room room, Character character)
    {
        if (character != identity) return;
        currentRoom = room;

    }
}

public enum Character
{
    Butler, Lord, Detective, Tycoon, General, Gardener, Imposter
}

public enum CharacterState
{
    Normal, Sleeping, Special1, Special2
}

public enum CharacterEmotion
{
    Standart, Shocked, Serious, Emotional, Content
}
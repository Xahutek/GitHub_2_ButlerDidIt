using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event")]
public class EventProfile : ScriptableObject
{
    public string SceneName;
    public Vector2 availableTime = new Vector2(0, 0.5f);
    public float duration
    {
        get { return availableTime.y - availableTime.x; }
    }

    [Header("Dialogue - All of these should be available all day. 'seen' & 'unqiue' will be overridden for event flow purposes")]
    public string introduction;
    public Dialogue StartDialogue;
    public Dialogue[] EventDialogue;
    public Dialogue NullDialogueReaction;

    public bool Triggered()
    {
        bool t = true;
        if (!Clock.HourPassed(availableTime.x))
            t = false;

        if (GameManager.isPaused)
            t = false;

        return t;
    }
}

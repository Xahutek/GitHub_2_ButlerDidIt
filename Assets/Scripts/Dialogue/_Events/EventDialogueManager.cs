using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDialogueManager : DialogueManager
{
    public EventProfile profile;

    protected override void Awake()
    {
            eventMain = this;

        if(profile)
            foreach (Dialogue dialogue in profile.EventDialogue)
            {
                dialogue.seen = false;
                dialogue.unique = dialogue.Trigger_Optional==null;
            }
    }

    public override Dialogue GetClueDialogue(Clue input=null)
    {
        Dialogue[] dialogues = profile.EventDialogue;
        List<Dialogue> options = new List<Dialogue>();
        foreach (Dialogue d in dialogues)
        {
            if (d.isValid(input))
                options.Add(d);
        }
        if (options.Count == 0)
            return input ? profile.NullDialogueReaction : null;

        return options[0];
    }

    public override void Close()
    {
        if (hardEscape)
        {
            hardEscape = false;
            return;
        }

        main = normalMain;
        base.Close();
    }
}

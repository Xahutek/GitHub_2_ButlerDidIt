using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_PresentationSkipScript : MonoBehaviour
{
    public float[] times;

    public CharacterProfile[] KnownCharacters;
    public Clue[] KnownClues, KnownCluesDetective;

    public Clue confessionClue;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.N)) SkipToNext();
        if (Input.GetKeyUp(KeyCode.M)) Done();
    }

    public void SkipToNext()
    {
        foreach (Clue c in KnownCluesDetective)
        {
            c.MakeKnownTo(Character.Detective);
        }
        foreach (Clue c in KnownClues)
        {
            c.MakeKnownTo(Character.Butler);
        }
        foreach (CharacterProfile c in KnownCharacters)
        {
            c.knownToPlayer = true;
        }

        foreach (float time in times)
        {
            if (Clock.Hour < time)
            {
                Clock.SetTime(time);
                EventSystem.main.RefreshRooms();
                return;
            }
        }
    }
    public void Done()
    {
        confessionClue.MakeKnownTo(Character.Detective);
    }
}

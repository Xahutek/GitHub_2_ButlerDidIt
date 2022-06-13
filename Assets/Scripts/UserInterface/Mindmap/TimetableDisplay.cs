using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimetableDisplay : ClueInfoKnot
{
    public CharacterProfile characterProfile;
    public Transform layoutParent;
    public TimetableEntry entryPrefab;
    public List<TimetableEntry> entries= new List<TimetableEntry>();

    public override void Refresh()
    {
        base.Refresh();

        int children=layoutParent.childCount;
        for (int i = 0; i < children; i++)
        {
            Destroy(layoutParent.GetChild(0).gameObject);
        }

        if (characterProfile.Timetable.Length > 1)
            for (int i = 1; i < characterProfile.Timetable.Length; i++)
            {
                CharacterProfile.TimeBox box = characterProfile.Timetable[i];
                if (box.important)
                {
                    TimetableEntry newEntry = Instantiate(entryPrefab, layoutParent);
                    newEntry.Refresh(box);
                }
            }
    }
}

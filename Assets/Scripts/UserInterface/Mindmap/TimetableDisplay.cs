using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimetableDisplay : ClueInfoKnot
{
    public CharacterProfile characterProfile;
    public Transform layoutParent;
    public TimetableEntry entryPrefab;
    public List<TimetableEntry> entries= new List<TimetableEntry>();

    public override void Refresh()
    {
        base.Refresh();

        for (int i = 0; i < entries.Count; i++)
        {
            Destroy(entries[i].gameObject);
        }
        entries.Clear();

        if (characterProfile.Timetable.Length > 1)
            for (int i = 1; i < characterProfile.Timetable.Length; i++)
            {
                CharacterProfile.TimeBox box = characterProfile.Timetable[i];
                if (box.important)
                {
                    TimetableEntry newEntry = Instantiate(entryPrefab, layoutParent);
                    newEntry.Refresh(box);
                    entries.Add(newEntry);
                }
            }
    }

    public override void ToggleEvent(bool on, float duration, Vector3 mid)
    {
        if (on) Refresh();

        float delay = on ? Mathf.Clamp01((transform.position - mid).magnitude / 3) : 0;
        Debug.Log(delay);

        Vector3
            OnPosition = new Vector3(-827.5f, -318f, 0f),
            OffPosition = new Vector3(-1100f, -318f, 0f);

        DOTween.Kill(tween);
        transform.localPosition = (!on ? OnPosition : OffPosition);
        tween = transform.DOLocalMove((on ? OnPosition : OffPosition), duration).OnComplete(() => Refresh())
            .SetEase(on ? Ease.InOutSine : Ease.InSine)
            .SetDelay(delay);
    }
}

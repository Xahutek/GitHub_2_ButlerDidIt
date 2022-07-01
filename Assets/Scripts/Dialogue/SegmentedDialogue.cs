using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue (Segmented)")]
public class SegmentedDialogue : Dialogue
{
    ////Base.Options displayed above
    //[System.Serializable] public class Segment
    //{
    //    public Clue clue;
    //    public Character knows;
    //}

    [Header("Segments - Lines will be overwritten by this!")]
    public Segment[] Segments= new Segment[0];
    [System.Serializable] public class Segment
    {
        public string description;
        [System.Serializable] public class Trigger
        {
            [Header("Clue Knowledge")]
            public bool activateClueTrigger;
            public Clue clue;
            public Character[] 
                knowingCharacters,
                unknowingCharacters;

            [Header("Suspicion Knowledge")]
            public bool activateSuspicionTrigger;
            public Character c;
            public SuspicionTriggerMode mode;
            public int suspicion;

            public enum SuspicionTriggerMode
            {
                Equal,EqualOrMore,EqualOrLess,ignoreButisLowest,ignoreButisHighest
            }

            public bool isTriggered()
            {
                bool triggered = true;

                if (activateClueTrigger)
                {
                    foreach (Character c in knowingCharacters)
                    {
                        if (!clue.KnownTo(c))
                        {
                            triggered = false; return triggered;
                        }
                    }
                    foreach (Character c in unknowingCharacters)
                    {
                        if (clue.KnownTo(c))
                        {
                            triggered = false; return triggered;
                        }
                    }
                }

                if (activateSuspicionTrigger)
                {
                    int actualSuspicion = SuspicionManager.main.GetSuspicion(c);
                    bool susValid = false;
                    switch (mode)
                    {
                        case SuspicionTriggerMode.Equal:
                            susValid = actualSuspicion == suspicion;
                            break;
                        case SuspicionTriggerMode.EqualOrMore:
                            susValid = actualSuspicion >= suspicion;
                            break;
                        case SuspicionTriggerMode.EqualOrLess:
                            susValid = actualSuspicion <= suspicion;
                            break;
                        case SuspicionTriggerMode.ignoreButisLowest:
                            susValid = SuspicionManager.main.GetLowest(out actualSuspicion) == c;
                            break;
                        case SuspicionTriggerMode.ignoreButisHighest:
                            susValid = SuspicionManager.main.GetHighest(out actualSuspicion) == c;
                            break;
                        default:
                            break;
                    }
                    if (!susValid) triggered = false;
                }

                return triggered;
            }
        }
        public Trigger trigger;
        public bool isTriggered()
        {
            return trigger.isTriggered();
        }

        public Line[] content= new Line[0];
    }

    public override void Begin()
    {
        base.Begin();
        List<Line> lines = new List<Line>();
        foreach (Segment T in Segments)
        {
            if (T.isTriggered())
                lines.AddRange(T.content);
        }
        Lines = lines.ToArray();
    }

}

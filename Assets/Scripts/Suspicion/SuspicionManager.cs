using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspicionManager : MonoBehaviour
{
    public static SuspicionManager main;

    [Header("Add Conclusions to generate Suspicion naturally or override them for testing.")]
    [SerializeField] bool overrideConclusions;
    [SerializeField]
    int
        OverrideButlerSuspicion = 4,
        OverrideGardenerSuspicion = 0,
        OverrideGeneralSuspicion = 0,
        OverrideTycoonSuspicion = 0;


    public Conclusion[]
        ButlerConclusions,
        GardenerConclusions,
        GeneralConclusions,
        TycoonConclusions;
    [System.Serializable] public class Conclusion
    {
        public Clue[] necessaryClues;
        public int suspicion;

        public bool isValid
        {
            get
            {
                foreach (Clue c in necessaryClues)
                {
                    if (!c.KnownAndSuspiciousTo(Character.Detective))
                        return false;
                }
                return true;
            }
        }
    }

    private void Awake()
    {
        main = this;
    }

    public int GetSuspicion(Character c)
    {
        Conclusion[] conclusions= new Conclusion[0];
        switch (c)
        {
            case Character.Butler:
                if (overrideConclusions) return OverrideButlerSuspicion;
                conclusions = ButlerConclusions;
                break;
            case Character.Tycoon:
                if (overrideConclusions) return OverrideTycoonSuspicion;
                conclusions = TycoonConclusions;
                break;
            case Character.General:
                if (overrideConclusions) return OverrideGeneralSuspicion;
                conclusions = GeneralConclusions;
                break;
            case Character.Gardener:
                if (overrideConclusions) return OverrideGardenerSuspicion;
                conclusions = GardenerConclusions;
                break;
            default:
                break;
        }

        int suspicion = 0;
        foreach (Conclusion con in conclusions)
        {
            if (con.isValid)
                suspicion += con.suspicion;
        }

        return suspicion;
    }
    public Character GetHighest(out int sus)
    {
        sus = -1;
        Character highest = Character.Butler;
        foreach (Character c in System.Enum.GetValues(typeof(Character)))
        {
            int thisSus = GetSuspicion(c);
            if (thisSus > sus)
            {
                sus = thisSus;
                highest = c;
            }
        }
        return highest;
    }
    public Character GetLowest(out int sus)
    {
        sus = -1;
        Character lowest = Character.Butler;
        foreach (Character c in System.Enum.GetValues(typeof(Character)))
        {
            int thisSus = GetSuspicion(c);
            if (thisSus <= sus)
            {
                sus = thisSus;
                lowest = c;
            }
        }
        return lowest;
    }

}

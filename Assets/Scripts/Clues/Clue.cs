using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Clue")]
public class Clue : ScriptableObject
{
    [TextArea] public new string name;
    public Sprite picture;

    [SerializeField]
    private Clue[]
        required,
        nullify;
    public bool seenInInventory = false;
    public bool
    isInventoryClue,
    isMindmapClue;

    [SerializeField]
    private bool
        presetButlerKnows,
        presetDetectiveKnows,
        presetInposterKnows,
        presetTycoonKnows,
        presetGeneralKnows,
        presetGardenerKnows;

    [Header("Will be overwritten! - These should be display and runtime use unly.")]
    [SerializeField] protected bool
        ButlerKnows;
    [SerializeField] protected bool
        DetectiveKnows,
        ImposterKnows,
        TycoonKnows,
        GeneralKnows,
        GardenerKnows;
    public Clue[] AlsoAffected;

    public void AlterKnown(Character C, bool k, bool editor = false)
    {
        if (KnownTo(C) != k)
        {
            switch (C)
            {
                case Character.Butler:
                    ButlerKnows = k; 
                    if (k) EventSystem.main.GetClue(this);
                    break;
                case Character.Detective: DetectiveKnows = k; break;
                case Character.Tycoon: TycoonKnows = k; break;
                case Character.General: GeneralKnows = k; break;
                case Character.Gardener: GardenerKnows = k; break;
                case Character.Imposter:
                    DetectiveKnows = k;
                    ImposterKnows = k;
                    break;
                default:
                    return;
            }
            ApplyAlsoAffected();
        }
    }
    public void MakeKnownTo(Character C)
    {
        if (!KnownTo(C))
        {
            switch (C)
            {
                case Character.Butler:
                    ButlerKnows = true;
                    EventSystem.main.GetClue(this);
                    break;
                case Character.Detective: DetectiveKnows = true; break;
                case Character.Tycoon: TycoonKnows = true; break;
                case Character.General: GeneralKnows = true; break;
                case Character.Gardener: GardenerKnows = true; break;
                case Character.Imposter:
                    DetectiveKnows = true;
                    ImposterKnows = true;
                    break;
                default:
                    return;
            }
            ApplyAlsoAffected();
        }
        InventoryUI.main.NewClue();
    }
    public void MakeUnknownTo(Character C)
    {
        if (KnownTo(C))
        {
            switch (C)
            {
                case Character.Butler: ButlerKnows = false; break;
                case Character.Detective: DetectiveKnows = false; break;
                case Character.Tycoon: TycoonKnows = false; break;
                case Character.General: GeneralKnows = false; break;
                case Character.Gardener: GardenerKnows = false; break;
                case Character.Imposter:
                    DetectiveKnows = false;
                    ImposterKnows = false;
                    break;
                default:
                    return;
            }
            ApplyAlsoAffected();
        }
        InventoryUI.main.NewClue();
    }

    public virtual void ApplyAlsoAffected()
    {
        if (AlsoAffected == null || AlsoAffected.Length == 0) return;
        foreach (Clue c in AlsoAffected)
        {
            if (c)
                foreach (Character character in System.Enum.GetValues(typeof(Character)))
                {
                    c.AlterKnown(character, KnownTo(character));
                }
        }
    }

    public bool KnownTo(Character C)
    {
        switch (C)
        {
            case Character.Butler:
                return ButlerKnows;
            case Character.Lord:
                return false;
            case Character.Detective:
                return DetectiveKnows;
            case Character.Tycoon:
                return TycoonKnows;
            case Character.General:
                return GeneralKnows;
            case Character.Gardener:
                return GardenerKnows;
            default:
                return false;
        }
    }
    public bool KnownAndSuspiciousTo(Character ch = Character.Detective)
    {
        if (!KnownTo(ch))
            return false;

        foreach (Clue c in required)
        {
            if (!c.KnownTo(ch))
                return false;
        }
        foreach (Clue c in nullify)
        {
            if (c.KnownTo(ch))
                return false;
        }
        return true;
    }
    //public float Weight(Character C)
    //{
    //    if (!KnownTo(C))
    //        return 0;
    //    foreach (Clue c in required)
    //    {
    //        if (!c.KnownTo(C))
    //            return 0;
    //    }
    //    foreach (Clue c in nullify)
    //    {
    //        if (c.KnownTo(C))
    //            return 0;
    //    }

    //    switch (C)
    //    {
    //        case Character.Butler:
    //            return ButlerWeight;
    //        case Character.Lord:
    //            return LordWeight;
    //        case Character.Detective:
    //            return DetectiveWeight;
    //        case Character.Tycoon:
    //            return TycoonWeight;
    //        case Character.General:
    //            return GeneralWeight;
    //        case Character.Gardener:
    //            return GardenerWeight;
    //        default:
    //            return 0;
    //    }
    //}

    public void FullReset()
    {
        ButlerKnows = presetButlerKnows;
        DetectiveKnows = presetDetectiveKnows;
        TycoonKnows = presetTycoonKnows;
        GeneralKnows = presetGeneralKnows;
        GardenerKnows = presetGardenerKnows;
    }
}

[System.Serializable] public class Note
{
    public Dialogue.Line line;

    public Note(Dialogue.Line L)
    {
        line = L;
    }
}
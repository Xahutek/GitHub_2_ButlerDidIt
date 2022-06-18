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
    public bool DisplayThoughtBubbleOnGain = true;
    public bool seenInInventory = false;

    [SerializeField] private bool
        _ButlerKnows,
        _DetectiveKnows,
        _InposterKnows,
        _TycoonKnows,
        _GeneralKnows,
        _GardenerKnows;

   [SerializeField]protected bool
        ButlerKnows,
        DetectiveKnows,
        ImposterKnows,
        TycoonKnows,
        GeneralKnows,
        GardenerKnows;

     //public float
     //   ButlerWeight,
     //   LordWeight,
     //   DetectiveWeight,
     //   TycoonWeight,
     //   GeneralWeight,
     //   GardenerWeight;

    public void AlterKnown(Character C, bool k, bool editor = false)
    {
        switch (C)
        {
            case Character.Butler: ButlerKnows = k; if (editor) ButlerKnows=_ButlerKnows; break;
            case Character.Detective: DetectiveKnows = k; if (editor) DetectiveKnows = _DetectiveKnows; break;
            case Character.Tycoon: TycoonKnows = k; if (editor) TycoonKnows = _TycoonKnows; break;
            case Character.General: GeneralKnows = k; if (editor) GeneralKnows = _GeneralKnows; break;
            case Character.Gardener: GardenerKnows = k; if (editor) GardenerKnows = _GardenerKnows; break;
            case Character.Imposter:
                DetectiveKnows = k;
                ImposterKnows = k;
                if (editor) DetectiveKnows = _DetectiveKnows;
                if (editor) ImposterKnows = _InposterKnows;
                break;
            default:
                return;
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
        }
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
        ButlerKnows = _ButlerKnows;
        DetectiveKnows = _DetectiveKnows;
        TycoonKnows = _TycoonKnows;
        GeneralKnows = _GeneralKnows;
        GardenerKnows = _GardenerKnows;
        seenInInventory = false;
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
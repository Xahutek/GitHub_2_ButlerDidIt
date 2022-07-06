using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public Clue Trigger_Optional;
    public Clue[] 
        NecessaryCluesKnown = new Clue[0],
        NecessaryCluesUnknown = new Clue[0];
    public bool nullDialogue,unique;[HideInInspector] public bool seen;
    public Vector2 AvailabilityHours = new Vector2(0, 24);

    public Character[] includedCharacters;
    public Line[] Lines;

    public bool isValid(Clue input)
    {
        bool valid = true;

        if (input != Trigger_Optional)
            valid = false;
        if (seen && unique)
            valid = false;
        float hour = Clock.Hour;
        if (hour < AvailabilityHours.x || AvailabilityHours.y < hour)
            valid = false;

        foreach (Clue c in NecessaryCluesKnown)
        {
            if (!c.KnownTo(Character.Butler))
            {
                valid = false;
                break;
            }
        }
        foreach (Clue c in NecessaryCluesUnknown)
        {
            if (c.KnownTo(Character.Butler))
            {
                valid = false;
                break;
            }
        }

        return valid;
    }
    public virtual void Begin()
    {
        foreach (Character c in includedCharacters)
        {
            c.Profile().knownToPlayer = true;
            if(Trigger_Optional)Trigger_Optional.MakeKnownTo(c);
        }
    }

    [System.Serializable]
    public class Line
    {
        public Character speaker;
        public CharacterEmotion speakerEmotion;
        public CharacterReaction[] otherReactions;
        public bool isThought;
        [System.Serializable]public class CharacterReaction
        {
            public Character character;
            public CharacterEmotion emotion;
        }

        [TextArea]public string text;
        public Clue fixedClue;
        public string spAnimTrigger;

        public Line(Character s, string t)
        {
            speaker = s;
            text = t;
        }

        public Line(Character s, string t, CharacterEmotion sE, CharacterReaction[] oE, Clue fC, string anim)
        {
            speaker = s;
            text = t;

            speakerEmotion = sE;
            otherReactions = oE;

            fixedClue = fC;

            spAnimTrigger = anim;
        }

        public void OnDisplay()
        {
            if (fixedClue)
                fixedClue.MakeKnownTo(Character.Butler);
        }
    }

    public enum EndingType
    {
        Open, OpenQuestion, SpecificQuestion, Closed, Fixed
    }

    public EndingType ending;
    public Dialogue ResumeFixed;

    [Header("OpenQuestion")]
    public Option[] availableAnswers;

    public Dialogue GetOpenQuestionReaction(Clue c)
    {
        List<Option> answers = new List<Option>();
        foreach (Option O in availableAnswers)
        {
            if (O.trigger == c)
            {
                answers.Add(O);
                if (c is Item) { Item item = c as Item; item.givenAway = true; }
            }
        }
        if (answers.Count == 0)
            return null;
        Option answer = answers[Random.Range(0, answers.Count)];
        OnPickOption(answer);
        return answer.reaction? answer.reaction: ResumeFixed;
    }

    [Header("SpecificQuestion - No more than 3 options!")]
    public Option[] options;

    [System.Serializable]public class Option
    {
        public string text;
        public Dialogue reaction;
        public Clue trigger;

        [Header("Rewards - makes known clue to all conversation partners")]
        public Clue[] rewards= new Clue[0];

        public void Reward(Dialogue d)
        {
            foreach (Clue cl in rewards)
            {
                foreach (Character c in d.includedCharacters)
                {
                    cl.MakeKnownTo(c);
                }
            }
        }
    }

    public virtual void OnPickOption(Option o)
    {
        o.Reward(this);
    }
}

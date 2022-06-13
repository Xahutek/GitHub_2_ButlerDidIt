using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Cinemachine;

public class DialogueManager : MonoBehaviour
{
    public TextParser parser;
    public static DialogueManager main,normalMain,eventMain;
    [Range(0.1f,2f)]public float speed = 1;
    public static bool isOpen;

    public InventoryUI inventoryUI;
    private PlayerController playerReference;
    [HideInInspector]
    public List<InteractableCharacter> CharacterObjects;
    public List<Character> Characters;

    public int maxDisplayedContent=4;
    [HideInInspector]public int CameraHorizontalOffset=0;
    public float
        BaseScale=1f,
        VerticalBaseHeight=1.25f,
        BoxVerticalSpacing,
        BoxHorizontalSpacing;
    float BoxHorizontalSign;

    public Vector2 Locus
    {
        get
        {
            Vector2 pos = transform.position;
            if (CharacterObjects.Count >= 0)
            {
                float totalX = 0f;
                float totalY = 0f;
                foreach (var C in CharacterObjects)
                {
                    totalX += C.transform.position.x;
                    totalY += C.transform.position.y;
                }
                float centerX = totalX / CharacterObjects.Count;
                float centerY = totalY / CharacterObjects.Count;

                pos = new Vector2(centerX, centerY);
            }

            if (CharacterObjects.Contains(playerReference))
                pos.y = playerReference.groundPosition.y;

            return pos;
        }
    }

    public Dialogue dialogue;
    public int line;

    public Transform dialogueCameraLocus;
    public List<SpeechBubble> bubbles = new List<SpeechBubble>();
    public bool arrivedAtEnd;
    public Bubble InputBubble;
    public OptionBubble[] OptionsBubbles;
    bool wasCleared, NextQueued;

    protected virtual void Awake()
    {
        if (main == null)
        {
            main = this;
            normalMain = this;
        }
        TextParser.main = parser;
    }
    private void Start()
    {
        playerReference = PlayerController.main;

        EventSystem.main.OnPickClue += OnPickClue;
    }
    private void Update()
    {
        bool NextAction = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return);

        if ((NextAction || NextQueued) && isOpen && !arrivedAtEnd)
        {
            if (!NextQueued && currentlyTyping != null && currentlyTyping.isTyping)
                SkipTypewriter();
            else if (!isRefreshing)
                NextBubble();
            else NextQueued = true;
        }
        
        if (isOpen && (!isRefreshing && Input.GetKeyDown(KeyCode.Escape)))
            Close();
        if (!isOpen && !wasCleared)
            Clear();

        if (isOpen && main == this) 
            dialogueCameraLocus.position = Locus + Vector2.left * inventoryUI.OpenStateLerp;
    }

    public void Open(InteractableCharacter B, Dialogue D) => Open(playerReference, B, D);
    public void Open(InteractableCharacter A, InteractableCharacter B, Dialogue D) => Open(new InteractableCharacter[2] { A, B }, D);
    public void Open(InteractableCharacter[] _Characters, Dialogue D)
    {
        main = this;

        if (isOpen) return;
        isOpen = true;
        wasCleared = false;

        CharacterObjects = new List<InteractableCharacter>();
        Characters = new List<Character>();

        CharacterObjects.AddRange(_Characters);
        foreach (InteractableCharacter c in CharacterObjects)
        {
            Characters.Add(c.character);
        }
        foreach (InteractableCharacter I in CharacterObjects)
        {
            I.inDialogue = false;
            I.isTalking = false;
        }

        dialogueCameraLocus.gameObject.SetActive(eventMain!=this);

        if (CharacterObjects.Count == 2)
            BoxHorizontalSign = BoxHorizontalSpacing * (CharacterObjects[0].transform.position.x < CharacterObjects[1].transform.position.x ? 1 : -1);

        SetDialogue(D);
    }
    public virtual void Close()
    {
        if (!isOpen) return;
        isOpen = false;

        foreach (InteractableCharacter I in CharacterObjects)
        {
            I.inDialogue = false;
            I.isTalking = false;
        }

        bool isQuestion = dialogue.ending != Dialogue.EndingType.Open && dialogue.ending != Dialogue.EndingType.Closed && dialogue.ending != Dialogue.EndingType.Fixed;
        if (!isQuestion) dialogue.seen = true;

        dialogueCameraLocus.gameObject.SetActive(false);

        Clear();
        inventoryUI.Close();

    }

    Character theOneWhoAsked;
    public virtual Dialogue GetClueDialogue(Clue C)
    {
        return theOneWhoAsked.Profile().GetDialogue(C);
    }
    public void OnPickClue(Clue C)
    {
        if (!arrivedAtEnd || !Characters.Contains(Character.Butler)||!isOpen) return;
        Dialogue nextDialogue;

        inventoryUI.Close();

        switch (dialogue.ending)
        {
            case Dialogue.EndingType.Open:
                nextDialogue = GetClueDialogue(C);
                SetDialogue(nextDialogue);
                break;
            case Dialogue.EndingType.OpenQuestion:
                nextDialogue = dialogue.GetOpenQuestionReaction(C);
                if (nextDialogue == null) nextDialogue = theOneWhoAsked.Profile().nullDialogueReaction;
                SetDialogue(nextDialogue);
                break;
            case Dialogue.EndingType.SpecificQuestion:
                break;
            default: // Closed
                break;
        }
    }
    public virtual void OnPickOption(Dialogue.Option option)
    {
        if (option == null || (!option.reaction && !dialogue.ResumeFixed)) 
        {
            OnPickClue(null);
            return;
        }
        foreach (Dialogue.Option o in dialogue.options)
        {
            if (o == option) dialogue.OnPickOption(o);
        }

        DialogueManager.main.SetDialogue(option.reaction? option.reaction:dialogue.ResumeFixed);
    }
    public void SetDialogue(Dialogue D)
    {
        if(D==null)
        {
            Close();
            return;
        }
        if (dialogue != null)
        {
            bool isQuestion = dialogue.ending != Dialogue.EndingType.Open && dialogue.ending != Dialogue.EndingType.Closed && dialogue.ending != Dialogue.EndingType.Fixed;
            if (!isQuestion||!D.nullDialogue) dialogue.seen = true;
        }

        Debug.Log("Start Dialogue " + D.name);

        dialogue = D;
        line = -1;
        arrivedAtEnd = false;

        ClearOptions();
        InputBubble.Disappear();

        dialogue.Begin();
        NextBubble();
    }
    public void SetEmotion(Character C, CharacterEmotion E, bool isTalking)
    {
        foreach (InteractableCharacter I in CharacterObjects)
        {
            if (I.character == C)
            {
                I.SetAnimation(E);
                I.isTalking = isTalking;
            }
        }
    }

    public void SkipTypewriter()
    {
        currentlyTyping.ForceEnd();
        currentlyTyping = null;
    }
    public void NextBubble()
    {
        NextQueued = false;
        if (!arrivedAtEnd)
        {
            line++;
            bubbles.Insert(0, bubbles[bubbles.Count - 1]);
            bubbles.RemoveAt(bubbles.Count - 1);
        }

        Refresh();
    }

    bool isRefreshing=false;
    Coroutine RefreshRoutine=null;
    SpeechBubble currentlyTyping;
    public void Refresh()
    {
        if (isRefreshing) return;
        if (RefreshRoutine != null) StopCoroutine(RefreshRoutine);
        RefreshRoutine = StartCoroutine(ExecuteRefresh());
    }
    public IEnumerator ExecuteRefresh()
    {
        if (isRefreshing) yield break;
        isRefreshing = true;

        if (dialogue == null)
        {
            Clear();
            isRefreshing=false;
            yield break;
        }

        //Get Lines
        List<Dialogue.Line> lines = new List<Dialogue.Line>();
        lines.AddRange(dialogue.Lines);

        arrivedAtEnd = line >= lines.Count;
        float height = VerticalBaseHeight;

        foreach (Bubble B in bubbles)
        {
            B.transform.GetChild(0).localScale = Vector3.one * BaseScale;
        }
        foreach (Bubble B in OptionsBubbles)
        {
            B.transform.GetChild(0).localScale = Vector3.one * BaseScale;
        }
        InputBubble.transform.GetChild(0).localScale = Vector3.one * BaseScale;


        //Refresh
        for (int i = 0; i < bubbles.Count; i++)
        {
            SpeechBubble B = bubbles[i];

            int line = this.line - i;
            bool isNew = i == 0;

            if (i >= Mathf.Min(maxDisplayedContent, bubbles.Count - 1)) //Above Limit
            {
                B.Disappear();
            }
            else if (line >= lines.Count) //End
            {
                Debug.Log("Arrive at End");

                Vector2 thisRoot = Locus + Vector2.left * BoxHorizontalSpacing * BoxHorizontalSign;

                Debug.Log(line);
                theOneWhoAsked = lines[line - 1].speaker;

                if (dialogue.ending == Dialogue.EndingType.Closed)
                {
                    isRefreshing = false;
                    Close();
                    yield break;
                }
                else if (dialogue.ending == Dialogue.EndingType.Fixed)
                {
                    isRefreshing = false;
                    if (dialogue.ResumeFixed != null)
                        SetDialogue(dialogue.ResumeFixed);
                    else Close();
                    yield break;
                }
                else if (dialogue.ending == Dialogue.EndingType.SpecificQuestion) //Show Options
                {
                    Dialogue.Option[] options = dialogue.options;
                    for (int o = 0; o < OptionsBubbles.Length; o++)
                    {
                        Dialogue.Option option = o < options.Length ? options[o] : null;
                        if (option != null && option.trigger != null && !option.trigger.KnownTo(Character.Butler)) option = null;

                        OptionBubble O = OptionsBubbles[o];
                        O.Refresh(option, thisRoot, height);

                        height += o < options.Length ? O.height * BaseScale + BoxVerticalSpacing : 0;
                    }
                }
                else //Open or Open Question
                {
                    inventoryUI.Open();
                    InputBubble.Appear(thisRoot, height);
                    height += InputBubble.height * BaseScale + BoxVerticalSpacing;
                }

                height -= B.height * BaseScale + BoxVerticalSpacing;
            }
            else if (line >= 0) //Positive > Line of this Dialogue
            {
                Character speaker = lines[line].speaker;

                Vector2 thisRoot = Locus;
                if(CharacterObjects.Count==2)
                {
                    thisRoot += (speaker == Characters[0] ? Vector2.left : Vector2.right)
                    * BoxHorizontalSpacing * BoxHorizontalSign;
                }
                else if(Characters.Contains(speaker))
                {
                    thisRoot = CharacterObjects[Characters.IndexOf(speaker)].transform.position;
                }
                    
                B.Refresh(lines[line], thisRoot, height,isNew);

                if (isNew)
                {
                    currentlyTyping = B;
                    lines[line].OnDisplay();
                    SetEmotion(lines[line].speaker,lines[line].speakerEmotion,true);

                    foreach (Dialogue.Line.CharacterReaction R in lines[line].otherReactions)
                    {
                        SetEmotion(R.character,R.emotion,false);
                    }
                }
            }
            else //Negative > Remaining Line from last Dialogue
            {
                B.Adjust(height);
            }

            yield return new WaitForFixedUpdate(); //Wait for Box 

            height += B.height * BaseScale + BoxVerticalSpacing;
        }

        yield return new WaitForSeconds(0.55f*speed);
        isRefreshing = false;
    }
    public void Clear()
    {
        if(RefreshRoutine!=null) StopCoroutine(RefreshRoutine);
        isRefreshing=false;

        dialogue = null;
        Characters = new List<Character>();
        CharacterObjects = new List<InteractableCharacter>();

        for (int i = 0; i < bubbles.Count; i++)
        {
            SpeechBubble B = bubbles[i];

            B.Disappear();
            B.line = null;
        }

        ClearOptions();

        InputBubble.Disappear();
        wasCleared = true;

        //inventoryUI.Close();
        //GameManager.isPaused = true;
    }
    public void ClearOptions()
    {
        for (int i = 0; i < OptionsBubbles.Length; i++)
        {
            OptionBubble B = OptionsBubbles[i];

            B.Disappear();
            B.option = null;
        }
    }

}

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
    public SoundManager soundManager;
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
    public SpeechBubble ClueBubble;
    public OptionBubble[] OptionsBubbles;
    bool wasCleared, NextQueued;

    protected virtual void Awake()
    {
            main = this;
            normalMain = this;
        TextParser.main = parser;
    }
    public virtual void Start()
    {
        playerReference = PlayerController.main;

        EventSystem.main.OnPickClue += OnPickClue;
        soundManager = SoundManager.main;
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
        
        if (isOpen && main == this && (!isRefreshing && Input.GetKeyDown(KeyCode.Escape)))
        {
            hardEscape = true;
            Close();
        }
        if (!isOpen && !wasCleared)
            Clear();

        if (isOpen && main == this && dialogueCameraLocus) 
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
        hardEscape = false;

        CharacterObjects = new List<InteractableCharacter>();
        Characters = new List<Character>();

        CharacterObjects.AddRange(_Characters);
        foreach (InteractableCharacter c in CharacterObjects)
        {
            Characters.Add(c.character);
        }
        foreach (InteractableCharacter I in CharacterObjects)
        {
            I.inDialogue = true;
            I.isTalking = false;
        }

        if(dialogueCameraLocus)
        dialogueCameraLocus.gameObject.SetActive(eventMain!=this);

        if (CharacterObjects.Count == 2)
            BoxHorizontalSign = BoxHorizontalSpacing * (CharacterObjects[0].transform.position.x < CharacterObjects[1].transform.position.x ? 1 : -1);

        SetDialogue(D);
    }

    protected bool hardEscape;
    public virtual void Close()
    {
        if (!isOpen) return;
        isOpen = false;

        if (!hardEscape)
        {
            bool isQuestion = dialogue.ending != Dialogue.EndingType.Open && dialogue.ending != Dialogue.EndingType.Closed && dialogue.ending != Dialogue.EndingType.Fixed;
            if (!isQuestion) dialogue.seen = true;
        }

        foreach (InteractableCharacter I in CharacterObjects)
        {
            I.RefreshLocus();
            I.inDialogue = false;
            I.isTalking = false;
        }

        if (dialogueCameraLocus)
            dialogueCameraLocus.gameObject.SetActive(false);
        soundManager.OnSpeakCharacter(Character.Butler);

        Clear();
        if(inventoryUI)inventoryUI.Close();

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

        switch (dialogue.ending)
        {
            case Dialogue.EndingType.Open:
                nextDialogue = GetClueDialogue(C);
                SetDialogue(nextDialogue);
                break;
            case Dialogue.EndingType.OpenQuestion:
                nextDialogue = dialogue.GetOpenQuestionReaction(C);
                if (nextDialogue == null) return;
                SetDialogue(nextDialogue);
                break;
            case Dialogue.EndingType.SpecificQuestion:
                break;
            default: // Closed
                break;
        }

        if (inventoryUI) inventoryUI.Close();
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
        ClueBubble.Disappear();

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
        SetEmotion(currentlyTyping.line.speaker, currentlyTyping.line.speakerEmotion, false);

        currentlyTyping.ForceEnd();
        currentlyTyping = null;
    }
    public void NextBubble()
    {
        NextQueued = false;
        if (!arrivedAtEnd&&!wasIntermitted)
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
    public Clue gainedClue=null;
    public bool wasIntermitted;
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

        bool hasClueQueued = gainedClue != null;
        arrivedAtEnd = line >= lines.Count&&!hasClueQueued;
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
        ClueBubble.transform.GetChild(0).localScale = Vector3.one * BaseScale;

        if (gainedClue == null&&ClueBubble.isOpen)
        {
            ClueBubble.Disappear();
            yield return new WaitForSeconds(0.75f*speed);
        }

        wasIntermitted = false;

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
            else if (line >= lines.Count&&!hasClueQueued) //End
            {
                Debug.Log("Arrive at End");

                Vector2 thisRoot = Locus + Vector2.left * BoxHorizontalSpacing * BoxHorizontalSign;

                Debug.Log(line);


                if (dialogue.ending == Dialogue.EndingType.Closed)
                {
                    isRefreshing = false;
                    Close();
                    yield break;
                }
                else if (dialogue.ending == Dialogue.EndingType.Fixed)
                {
                    isRefreshing = false;

                    height -= B.height * BaseScale + BoxVerticalSpacing;

                    if (dialogue.ResumeFixed != null)
                        SetDialogue(dialogue.ResumeFixed);
                    else Close();

                    yield break;
                }
                else if (dialogue.ending == Dialogue.EndingType.SpecificQuestion) //Show Options
                {
                    Dialogue.Option[] options = dialogue.options;
                    ClearAnimations();
                    if (this==eventMain)
                    foreach (InteractableCharacter c in CharacterObjects)
                    {
                        if (c.character == Character.Butler)
                        {
                            thisRoot = c.transform.position;

                            break;
                        }
                    }
                    int activeOptions=0;
                    for (int o = 0; o < OptionsBubbles.Length; o++)
                    {
                        Dialogue.Option option = o < options.Length ? options[o] : null;
                        if (option != null && option.trigger != null && !option.trigger.KnownTo(Character.Butler)) option = null;

                        OptionBubble O = OptionsBubbles[o];
                        O.Refresh(option, thisRoot, height);

                        if (option != null)
                        {
                            activeOptions++;

                            yield return new WaitForFixedUpdate();

                            height += o < options.Length ? O.height * BaseScale + BoxVerticalSpacing : 0;
                        }
                    }

                    Debug.Log(activeOptions);

                    if (activeOptions == 0)
                    {
                        height -= B.height * BaseScale + BoxVerticalSpacing;

                        if (dialogue.ResumeFixed != null)
                            SetDialogue(dialogue.ResumeFixed);
                        else Close();

                        yield break;
                    }
                }
                else //Open or Open Question
                {
                    if (inventoryUI) inventoryUI.Open();
                    InputBubble.Appear(thisRoot, height);
                    height += InputBubble.height * BaseScale + BoxVerticalSpacing;

                    ClearAnimations();
                }

                height -= B.height * BaseScale + BoxVerticalSpacing;

            }
            else if (line >= 0||hasClueQueued) //Positive > Line of this Dialogue
            {
                Vector2 thisRoot = Locus;

                if (isNew && hasClueQueued)
                {
                    //Vector2 thisRoot = Locus;
                    Dialogue.Line L = new Dialogue.Line(Character.Butler, gainedClue.name);
                    ClueBubble.Refresh(this, L, thisRoot, height, true);

                    currentlyTyping = ClueBubble;

                    gainedClue = null;
                    wasIntermitted = true;

                    ClearAnimations();

                    yield return new WaitForFixedUpdate();

                    height += ClueBubble.height * BaseScale + BoxVerticalSpacing;

                    continue;
                }
                else if (lines.Count > line && line >= 0) 
                {
                    if (lines[line].speaker != Character.Butler)
                        theOneWhoAsked = lines[line].speaker;

                    if (CharacterObjects.Count == 2)
                    {
                        thisRoot += (lines[line].speaker == Characters[0] ? Vector2.left : Vector2.right)
                        * BoxHorizontalSpacing * BoxHorizontalSign;
                    }
                    else if (Characters.Contains(lines[line].speaker))
                    {
                        Vector2 characterPosition = CharacterObjects[Characters.IndexOf(lines[line].speaker)].transform.position;
                        RaycastHit2D hitGroundLong = Physics2D.Raycast(characterPosition, Vector2.down, 3, PlayerController.main.LMWalls);

                        thisRoot = new Vector2((hitGroundLong ? hitGroundLong.point : characterPosition).x,thisRoot.y);
                    }

                    B.Refresh(this,lines[line], thisRoot, height, isNew);

                    if (isNew)
                    {
                        currentlyTyping = B;
                        if(lines[line].speaker!=Character.Butler) soundManager.OnSpeakCharacter(lines[line].speaker);
                        RefreshAnimations(lines[line]);
                        lines[line].OnDisplay();
                    }
                }
            }
            else //Negative > Remaining Line from last Dialogue
            {
                B.Adjust(height);
                if(!B.isOpen)
                    height -= B.height * BaseScale + BoxVerticalSpacing;
            }

            if (wasIntermitted) line--;

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

        ClueBubble.Disappear();
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

    public void ClearAnimations()
    {
        foreach (Character c in Characters)
        {
            SetEmotion(c, CharacterEmotion.Standart, false);
        }
    }
    public void RefreshAnimations(Dialogue.Line L)
    {
        ClearAnimations();
        if(L.spAnimTrigger == "")
        {
            SetEmotion(L.speaker, L.speakerEmotion, !L.isThought);

            if (L.fixedClue && L.fixedClue.isInventoryClue && !L.fixedClue.KnownTo(Character.Butler))
                gainedClue = L.fixedClue;

            foreach (Dialogue.Line.CharacterReaction R in L.otherReactions)
            {
                SetEmotion(R.character, R.emotion, false);
            }
        }
        else
        {
            foreach (InteractableCharacter I in CharacterObjects)
            {
                if (I.character == L.speaker)
                { 
                    I.animator.SetTrigger(L.spAnimTrigger);
                }
            }
        }
    }

}

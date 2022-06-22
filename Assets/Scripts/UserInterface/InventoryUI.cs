using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] bool hideUnknownCharacters;

    public static InventoryUI main;
    RectTransform rectTransform;

    public ClueDisplay 
        ClueDisplayPrefab,
        ItemDisplayPrefab;

    public static bool isOpen;
    public    bool showCharas;

    public float moveDuration;

    public Character currentCharacter,
        nextC,previousC;
    public List<ClueDisplay> ClueDisplays = new List<ClueDisplay>();
    public List<ClueDisplay> ItemDisplays = new List<ClueDisplay>();
    public Image
        PortraitMain,
        PortraitLeft,
        PortraitRight;
    public TMP_Text Name, ItemsHeader;
    public RectTransform CharasMarker, ItemMarker, paperBottom;
    public Button[] ToggleButton;
    public DialogueManager dialogueManager;
    public AudioClip paperOpen, paperTurn;

    private void Awake()
    {
        if(main==null)
            main=this;
        rectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        Close();
        showCharas = true;
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    private void Update()
    {
        if (((Input.GetKeyUp(KeyCode.I)|| Input.GetKeyUp(KeyCode.E) || ((Input.GetKeyUp(KeyCode.Escape) && isOpen))) &&!DialogueManager.isOpen))
        {
            if (isOpen) Close();
            else Open();
        }
        foreach (Button tButton in ToggleButton) 
        {
            if (!isOpen)
            {
                tButton.enabled = !DialogueManager.isOpen;
            }
            else tButton.enabled = true;
        }
        Peek();
    }

    public bool isPeeking;
    public void Peek()
    {
        bool peeking= Input.mousePosition.x > Screen.width / 5.0f;
        if (peeking!=isPeeking&&!isOpen)
        {
            DOTween.Kill(tween);

            if (!isPeeking)
                tween = rectTransform.DOAnchorPosX(-300f, moveDuration*0.5f).SetEase(Ease.InOutSine);
            else
                tween = rectTransform.DOAnchorPosX(-235f, moveDuration*0.5f).SetEase(Ease.InOutSine);
            isPeeking =peeking;
        }
    }
    public void Refresh()
    {
        Clear();

        CharacterProfile profile = currentCharacter.Profile();

        Clue[] clueList = profile.connectedClues;
        Note[] noteList = ClueLibrary.main.AllNotes.ToArray();

        int siblingI = 0,noteI=0;
        for (int i = 0; i < Mathf.Max(clueList.Length, noteList.Length, ClueDisplays.Count); i++)
        {
            Note n = null;
            Clue c = i < clueList.Length ? clueList[i] : null;
            if (c!=null&&!c.KnownTo(Character.Butler)) continue;
            if (c == null && noteI < noteList.Length)
            {
                n = noteList[noteI];
                n = n.line.speaker == currentCharacter ? n : null;
                noteI++;
                if (n == null)
                {
                    i--;
                    continue;
                }
            }
            if (i >= ClueDisplays.Count)
                ClueDisplays.Add(Instantiate(ClueDisplayPrefab, ClueDisplays[i - 1].transform.parent));
            siblingI++;
            ClueDisplays[i].transform.SetSiblingIndex(siblingI);

            if(c) ClueDisplays[i].Refresh(c, true);
            else ClueDisplays[i].Refresh(n);
        }
        NewClue();

        foreach (ClueDisplay CD in ItemDisplays)
        {
            CD.Clear();
        }
    }
    public void Clear()
    {
        foreach (ClueDisplay c in ClueDisplays)
        {
            c.Clear();
        }
    }

    int charasMarkerTweenID, itemMarkerTweenID;
    public void ShowCharas()
    {
        if (!isOpen) { Open(); }
        else
        {
            // Should remove the Portraits
            PortraitMain.transform.parent.gameObject.SetActive(showCharas);
            PortraitLeft.transform.parent.gameObject.SetActive(showCharas);
            PortraitRight.transform.parent.gameObject.SetActive(showCharas);
            Name.gameObject.SetActive(showCharas);
            ItemsHeader.gameObject.SetActive(!showCharas);

            paperBottom.offsetMax = new Vector2(rectTransform.offsetMax.y, showCharas ? -250 : -150);

            MarkerMovement(showCharas);

            SoundManager.main.effectSource.PlayOneShot(paperTurn);

            if (showCharas)
            {
                Refresh();
            }
            else
            {
                Item[] itemList = ClueLibrary.main.AllItems;
                foreach (ClueDisplay CD in ClueDisplays)
                {
                    CD.Clear();
                }

                for (int i = 0; i < Mathf.Max(itemList.Length, ClueDisplays.Count); i++)
                {
                    Item I = i < itemList.Length ? itemList[i] : null;
                    if (I != null && !I.KnownTo(Character.Butler) && !I.givenAway) I = null;
                    if (i >= ItemDisplays.Count)
                        ItemDisplays.Add(Instantiate(ItemDisplayPrefab, ItemDisplays[i - 1].transform.parent));
                    ItemDisplays[i].transform.SetSiblingIndex(i);
                    ItemDisplays[i].Refresh(I, true);
                }
            }
        }
    }

    #region Markers
    private void MarkerMovement(bool status)
    {
        DOTween.Kill(charasMarkerTweenID);

        charasMarkerTweenID =
            ItemMarker.DOLocalMoveX(!status && isOpen || !isOpen ? 310 : 280, 0.2f).intId;
        itemMarkerTweenID =
            CharasMarker.DOLocalMoveX(status && isOpen|| !isOpen ? 310 : 280, 0.2f).intId;        
    }

    public void NewClue()
    {
        bool newClue = false;
        bool newItem = false;
        foreach (Clue clue in ClueLibrary.main.AllClues)
        {
            if (clue.KnownTo(Character.Butler) && !clue.seenInInventory)
            {
                if (clue is Item) { newItem = true; } 
                else if(clue.isInventoryClue) { newClue = true; }
            }
        }
        foreach (Item item in ClueLibrary.main.AllItems)
        {
            if (item.KnownTo(Character.Butler) && !item.seenInInventory)
            {
                newItem = true; 
            }
        }
        CharasMarker.GetComponentInChildren<TMP_Text>().enabled = newClue;
        ItemMarker.GetComponentInChildren<TMP_Text>().enabled = newItem;
    }
    #endregion

    public void OpenButlerInventory() => ChangeCurrentCharacter(Character.Butler);
    public void ChangeCurrentCharacter(bool right) => ChangeCurrentCharacter(right ? nextC : previousC);
    public void ChangeCurrentCharacter(Character c)
    {
        currentCharacter= c;

        List<Character> characters= new List<Character>();
        foreach (Character C in System.Enum.GetValues(typeof(Character)))
        {
            if ((C.Profile().knownToPlayer || !hideUnknownCharacters) || C == Character.Butler)
            {
                characters.Add(C);
                Debug.Log("Validated "+C);
            }
        }

        int
            current = characters.IndexOf(currentCharacter),
            next = current + 1,
            previous = current - 1;

        if (previous < 0) previous = characters.Count - 1;
        if (next >= characters.Count) next = 0;

        previousC = characters[previous];
        nextC = characters[next];

        //bool checksOut=false;
        //while (!checksOut && previousC != Character.Butler)
        //{
        //    previous -= 1;
        //    if (previous < 0) previous = characters.Count - 1;
        //    previousC = (Character)previous;
        //    checksOut = !hideUnknownCharacters || previousC.Profile().knownToPlayer;
        //}
        //checksOut = false;
        //while (!checksOut && nextC != Character.Butler)
        //{
        //    next += 1;
        //    if (next >=characters.Count) next = 0;
        //    nextC = (Character)next;
        //    checksOut = !hideUnknownCharacters || nextC.Profile().knownToPlayer;
        //}

        PortraitMain.sprite = currentCharacter.Profile().Portrait;
        PortraitLeft.sprite = previousC.Profile().Portrait;
        PortraitRight.sprite = nextC.Profile().Portrait;

        Name.text = currentCharacter.Profile().name;
        SoundManager.main.effectSource.PlayOneShot(paperTurn);

        Debug.Log("Changed Inventory to "+currentCharacter.ToString());

        ShowCharas();
    }

    #region MenuManagement
    public float OpenStateLerp = 0;
    Tween tween, numberTween;
    public void TapMarker(bool marker)
    {
        showCharas = marker;
        if (!isOpen) { Toggle(); } else { ShowCharas(); }
    }
    public void Toggle()
    {
        if (DialogueManager.isOpen && !isOpen) return;
        if (isOpen)
        {
            Close();
            if (DialogueManager.isOpen) { dialogueManager.Close(); }
        }
        else Open();
    }
    public void Open()
    {
        if (PlayerController.main.grounded)
        {
            isOpen = true;
            GameManager.isPaused = true;

            ChangeCurrentCharacter(currentCharacter);
            ShowCharas();

            SoundManager.main.effectSource.PlayOneShot(paperOpen);
            DOTween.Kill(tween);
            tween = rectTransform.DOAnchorPosX(300f, moveDuration).SetEase(Ease.InOutSine);
            DOTween.Kill(numberTween);
            numberTween = DOTween.To(() => OpenStateLerp, i => OpenStateLerp = i, 1, moveDuration*2f).SetEase(Ease.InOutSine);
        }
    }
    public void Close()
    {
        isOpen = false;
        GameManager.isPaused = false;
        MarkerMovement(false);
        DOTween.Kill(tween);
        tween = rectTransform.DOAnchorPosX(-300f, moveDuration).SetEase(Ease.OutSine);
        DOTween.Kill(numberTween);
        numberTween = DOTween.To(() => OpenStateLerp, i => OpenStateLerp = i, 0, moveDuration*2f).SetEase(Ease.OutSine);
    }

    #endregion
}

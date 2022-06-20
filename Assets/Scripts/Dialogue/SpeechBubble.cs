using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;


public class SpeechBubble : Bubble
{
    public TMP_Text text;
    [HideInInspector]public Dialogue.Line line;
    private Character currentChara;
    public Color butlerColor, evaColor, tyrellColor, ednaColor, gertieColor, susColor;
    public bool isTyping;
    public virtual void Refresh(DialogueManager manager,Dialogue.Line line, Vector2 Root, float heightStack, bool Typewriter=false)
    {
        this.line = line;

        if (line != null)
        {
            ForceEnd();
            text.text = line.text.CustomParse();

            if (Typewriter)
                TypewriterRoutine = StartCoroutine(ExecuteTypewriter(manager,line));
        }

        Show(Root, heightStack);
        CharacterColor();
        MakeTransparent();
    }

    Coroutine TypewriterRoutine = null;
    IEnumerator ExecuteTypewriter(DialogueManager manager,Dialogue.Line line)
    {
        isTyping = true;

        int
            fullCharactersShown = line.text.Length,
            charactersShown = 0;

        while (charactersShown<=fullCharactersShown)
        {
            text.maxVisibleCharacters = charactersShown;

            yield return new WaitForSeconds(0.025f*Random.Range(1,1.5f));
            charactersShown++;
        }
        text.maxVisibleCharacters = fullCharactersShown;

        if(manager!=null)//Null on clue pop up
            manager.SetEmotion(line.speaker, line.speakerEmotion, false);

        isTyping = false;
    }

    public virtual void ForceEnd()
    {
        if (TypewriterRoutine == null) return;
        StopCoroutine(TypewriterRoutine);
        text.maxVisibleCharacters = text.text.Length;

        isTyping = false;
    }

    public Image[] images;
    private void MakeTransparent()
    {
        Camera cam = FindObjectOfType<Camera>();
        Vector3 camPos = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight / 4, 0)); //start fade at upper fourth
        Vector3 upperBorder = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector2 bubblePos = RectTransformUtility.PixelAdjustPoint(transform.position, transform, GetComponentInParent<Canvas>());
        float value = (camPos.y / (bubblePos.y - upperBorder.y) - 1) / 100 * 4;

        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, Mathf.Lerp(0.5f, 1, value));
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(0.5f, 1, value));
        //Debug.Log(camPos.y + " / " + bubblePos.y + " = " + value);
    }

    private void CharacterColor()
    {        
        if(currentChara != line.speaker)
        {
            currentChara = line.speaker;
            switch (currentChara)
            {
                case Character.Butler:
                    images[1].color = butlerColor;
                    break;
                case Character.Detective:
                    images[1].color = evaColor;
                    break;
                case Character.Tycoon:
                    images[1].color = tyrellColor;
                    break;
                case Character.General:
                    images[1].color = ednaColor;
                    break;
                case Character.Gardener:
                    images[1].color = gertieColor;
                    break;
                case Character.Imposter:
                    images[1].color = susColor;
                    break;
                default:
                    images[1].color = butlerColor;
                    break;
            }
        }
    }

    //public bool hasBeenNoted = false;
    //public void NoteDown()
    //{
    //    if (line != null && line.speaker != Character.Butler)
    //    {
    //        EventSystem.main.MakeNote(new Note(line));
    //        hasBeenNoted = true;
    //    }
    //}
}

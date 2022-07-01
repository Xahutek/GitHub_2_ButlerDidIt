using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Letters
{
    public class GameManager : MinigameObject
    {
        public static GameManager main;

        public PaperSpot[] spots;
        
        public GameState gameState;
        public TMP_Text resultText;
        public Item EmptyLetters;
        public Item FilledLetters;
        public Clue BurntLetters;
        public Fire fire;

        public AudioClip burning;

        private void Awake()
        {
            main = this;
            spots = FindObjectsOfType<PaperSpot>();
        }

        public void CheckState()
        {                        
            switch (gameState)
            {
                case GameState.LettersObtained:
                    resultText.text = "I could burn those useless letters from Gertie.";
                    break;
                case GameState.Deciphered:
                    resultText.text = "The Lord had some impressive ways of communicating.";
                    break;
                case GameState.Burnt:
                    resultText.text = "Those letters are gone forever";
                    break;
                default: //not getting anywhere
                    resultText.text = "Fire... source of warmth and absolute destruction..";
                    LetterHold.main.gameObject.SetActive(false);
                    break;
            }
        }

        public void SetPaperState()
        {
            if(gameState != GameState.Deciphered)
            {
                if (ReadabilityCheck() >= 0.75f)
                {
                    gameState = GameState.Deciphered;
                }
                else
                {
                    gameState = GameState.LettersObtained;
                }
            }         
            CheckState();

        }

        private float ReadabilityCheck()
        {
            float alphaValue = 0;
            foreach (PaperSpot spot in spots)
            {
                alphaValue += spot.text.color.a;
            }
            return alphaValue / spots.Length;
        }
    
        public override void Open()
        {
            base.Open();
            if (BurntLetters.KnownTo(Character.Butler))
            {
                gameState = GameState.Burnt;
                LetterHold.main.gameObject.SetActive(false);
            }
            else if (FilledLetters.KnownTo(Character.Butler)) 
            { 
                gameState = GameState.Deciphered;
                EmptyLetters.givenAway = true;
                foreach(PaperSpot spot in spots) { spot.time = spot.minHoldTime + spot.completeTime; }
            }
            else if (EmptyLetters.KnownTo(Character.Butler))
            {
                gameState = GameState.LettersObtained;
            }
            CheckState();
            foreach (PaperSpot spot in spots)
            {
                spot.InkStatus();
            }
        }

        
        public override void Close()
        {
            CheckState();
            switch (gameState)
            {
                case GameState.Deciphered:
                    FilledLetters.MakeKnownTo(Character.Butler);
                    EmptyLetters.givenAway = true;
                    break;
                case GameState.Burnt:
                    EmptyLetters.givenAway = true;
                    FilledLetters.MakeKnownTo(Character.Butler);
                    FilledLetters.givenAway = true;
                    BurntLetters.MakeKnownTo(Character.Butler);
                    break;
                default: //no letters in inventory or burnt                    
                    break;
            }
            base.Close();
        }

        private bool noReturn = false;
        public void PaperBurning()
        {
            if (!noReturn)
            {
                noReturn = true;
                SoundManager.main.PlayOneShot(burning);
                LetterHold.main.transform.GetChild(0).SetParent(fire.transform);
                fire.enabled = false;
                gameState = GameState.Burnt;
                CheckState();
            }
        }
        public void RemoveText()
        {
            foreach (PaperSpot spot in spots)
            {
                spot.text.color = Color.clear;
            }
        }
    }

    public enum GameState
    {
        noLetters, LettersObtained, Deciphered, Burnt
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ChessBoard
{
    public class GameManager : MinigameObject
    {
        public static GameManager main;

        public GameState gameState;
        public TMP_Text resultText;

        public Clue LordWinsClue;

        public List<BoardSlot> slots;
        public List<ChessFigure> figures;


        private void Awake()
        {
            main = this;
        }
        public void CheckState()
        {
            bool ednaWon = CheckState(ChessColor.White, false) || CheckState(ChessColor.White, true);
            bool lordWon = CheckState(ChessColor.Black, false) || CheckState(ChessColor.Black, true);

            if (ednaWon && lordWon) gameState = GameState.Draw;
            else if (ednaWon) gameState = GameState.WhiteWins;
            else if (lordWon) gameState = GameState.BlackWins;
            else gameState = GameState.Playing;

            switch (gameState)
            {
                case GameState.Playing:
                    resultText.text = "Who am I trying to fool with this..?";
                    break;
                case GameState.Draw:
                    resultText.text = "This looks inconclusive.";
                    break;
                case GameState.BlackWins:
                    resultText.text = "One would almost believe the Lord finally won...";
                    break;
                case GameState.WhiteWins:
                    resultText.text = "Looks like White won... Edna presumably";
                    break;
                default:
                    break;
            }
        }
        public bool CheckState(ChessColor winner, bool flipped)
        {
            bool valid = true;

            foreach (BoardSlot slot in slots)
            {
                if (slot.isCrucial && !slot.isValid(winner, flipped))
                    valid = false;
            }

            return valid;
        }

        public override void Open()
        {
            base.Open();
            CheckState();
        }
        public override void Load()
        {
            List<BoardSlot> pointedSlots = slots;

            if (MinigameData.slotFigures==null||MinigameData.slotFigures.Count==0)
            {
                Debug.LogWarning("MinigameData Slotfigures is null or empty");
                return;
            }

            foreach (BoardSlot s in slots)
            {
                s.figure = null;
            }
            foreach (ChessFigure f in figures)
            {
                f.slot = null;
            }

            for (int i = 0; i < MinigameData.slotFigures.Count; i++)
            {
                Vector2Int save = MinigameData.slotFigures[i];

                BoardSlot s = slots[save.x];

                if (save.y == -1)
                    s.figure = null;
                else
                {
                    s.figure = figures[save.y];
                    figures[save.y].slot = s;
                }
            }
        }
        public override void Close()
        {
            CheckState();
            if (gameState == GameState.BlackWins)
                LordWinsClue.MakeKnownTo(Character.Butler);
            base.Close();
        }
        public override void Save()
        {
            MinigameData.SaveTo(this);
        }
    }

    public enum GameState
    {
        Playing, Draw, BlackWins, WhiteWins
    }
}
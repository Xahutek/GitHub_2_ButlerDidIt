using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ChessBoard
{
    public class GameManager : MinigameObject
    {
        public static GameManager main;

        public ChessFigure
            BlackKing,
            WhiteKing;
        public ChessFigure[]
            blackFigures,
            whiteFigures;
        public GameState gameState;
        public TMP_Text resultText;

        public Clue LordWinsClue;

        private void Awake()
        {
            main = this;
        }
        public void CheckState()
        {
            List<BoardSlot>
                BlackKingsSlots = new List<BoardSlot>(),
                WhiteKingSlots = new List<BoardSlot>();

            Dictionary<ChessFigure, List<BoardSlot>> keyValuePairs = new Dictionary<ChessFigure, List<BoardSlot>>();

            BlackKingsSlots.Add(BlackKing.slot);
            BlackKingsSlots.AddRange(BlackKing.slot.neighbours);

            WhiteKingSlots.Add(WhiteKing.slot);
            WhiteKingSlots.AddRange(WhiteKing.slot.neighbours);

            foreach (ChessFigure Fig in blackFigures)
            {
                BoardSlot[] infuence = Fig.GetFieldsOfInfluence();
                if (BlackKingsSlots.Contains(Fig.slot))
                    BlackKingsSlots.Remove(Fig.slot);
                foreach (BoardSlot inf in infuence)
                {
                    Debug.DrawLine(inf.transform.position, inf.transform.position - inf.transform.forward * 200, Color.black, 5);
                    if (WhiteKingSlots.Contains(inf))
                        WhiteKingSlots.Remove(inf);
                }
            }
            foreach (ChessFigure Fig in whiteFigures)
            {
                BoardSlot[] infuence = Fig.GetFieldsOfInfluence();
                if (WhiteKingSlots.Contains(Fig.slot))
                    WhiteKingSlots.Remove(Fig.slot);
                foreach (BoardSlot inf in infuence)
                {
                    Debug.DrawLine(inf.transform.position, inf.transform.position - inf.transform.forward * 100, Color.white, 5);
                    if (BlackKingsSlots.Contains(inf))
                        BlackKingsSlots.Remove(inf);
                }
            }

            foreach (BoardSlot s in WhiteKingSlots)
            {
                Debug.DrawLine(s.transform.position, s.transform.position - s.transform.forward * 300, Color.green, 5);
            }
            foreach (BoardSlot s in BlackKingsSlots)
            {
                Debug.DrawLine(s.transform.position, s.transform.position - s.transform.forward * 300, Color.green, 5);
            }

            bool
                BlackWon = WhiteKingSlots.Count == 0,
                WhiteWon = BlackKingsSlots.Count == 0;

            Debug.Log("Black won:" + BlackWon);
            Debug.Log("White won:" + WhiteWon);

            if (BlackWon && WhiteWon) gameState = GameState.Draw;
            else if (!BlackWon && WhiteWon) gameState = GameState.WhiteWins;
            else if (BlackWon && !WhiteWon) gameState = GameState.BlackWins;
            else gameState = GameState.Playing;

            switch (gameState)
            {
                case GameState.Draw:
                    resultText.text = "A draw. Already more than the Lord could ever accomplish...";
                    break;
                case GameState.BlackWins:
                    resultText.text = "Now you'd almost believe the Lord actually won...";
                    break;
                case GameState.WhiteWins:
                    resultText.text = "Looks like White won... Edna presumably.";
                    break;
                default: //Playing
                    resultText.text = "Looks like the game ended prematurely...";
                    break;
            }
        }

        public override void Open()
        {
            base.Open();
            CheckState();
        }
        public override void Load()
        {
            if (MinigameData.blackKing != null)
            {
                BlackKing.slot.figure = null;
                BlackKing.slot = BoardSlot.slots[MinigameData.blackKing.coordinates];
                BlackKing.slot.figure = BlackKing;
            }

            if (MinigameData.whiteKing != null)
            {
                WhiteKing.slot.figure = null;
                WhiteKing.slot = BoardSlot.slots[MinigameData.whiteKing.coordinates];
                WhiteKing.slot.figure = WhiteKing;
            }

            if (MinigameData.blackfigures != null)
                for (int i = 0; i < MinigameData.blackfigures.Length; i++)
                {
                    ChessFigure figure = blackFigures[i];
                    figure.slot.figure = null;
                    figure.slot = BoardSlot.slots[MinigameData.blackfigures[i].coordinates];
                    figure.slot.figure = figure;
                }

            if (MinigameData.whiteFigures != null)
                for (int i = 0; i < MinigameData.whiteFigures.Length; i++)
                {
                    ChessFigure figure = whiteFigures[i];
                    figure.slot.figure = null;
                    figure.slot = BoardSlot.slots[MinigameData.whiteFigures[i].coordinates];
                    figure.slot.figure = figure;
                }
        }
        public override void Close()
        {
            CheckState();
            if (gameState == GameState.BlackWins)
                LordWinsClue.MakeKnownTo(Character.Butler);
            BoardSlot.slots = new Dictionary<Vector2, BoardSlot>();
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

    //public struct BMap
    //{
    //    public int size;
    //    public int[,] map;

    //    public int Map(int x, int y)
    //    {
    //        return map[x, y];
    //    }
    //    public int centeredMap(int x, int y, bool reset=false)
    //    {
    //        int half = Mathf.FloorToInt((float)size * 0.5f);
    //        if (x + half >= size || y + half >= size) return 0;
    //        else return map[x + half, y + half];
    //    }

    //    public void SetUp(int size)
    //    {
    //        this.size = size;
    //        map = new int[size, size];
    //    }
    //}

    //public static class ChessBoardUtility
    //{
    //    public static BMap BoardMask(this BMap board, BMap mask, Vector2 offset)
    //    {
    //        for (int x = 0; x < board.size; x++)
    //        {
    //            for (int y = 0; y < board.size; y++)
    //            {
    //                int
    //                    boardValue= board.Map(x, y),
    //                    maskValue=mask.centeredMap(x-, y);
    //            }
    //        }
    //    }
    //}
}
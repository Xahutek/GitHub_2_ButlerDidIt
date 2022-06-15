using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calendar;

public static class MinigameData
{
    //Calendar
    public static List<bool> calendar;

    //Chess
    [System.Serializable]
    public class ChessFigure
    {
        public Vector2Int coordinates;

        public ChessFigure(Vector2 _coords)
        {
            coordinates = new Vector2Int(Mathf.FloorToInt(_coords.x), Mathf.FloorToInt(_coords.y));
        }
        public ChessFigure(ProgressFile.Minigames_ProgressFile.SerializedVector2 _coords)
        {
            coordinates = new Vector2Int(Mathf.FloorToInt(_coords.x), Mathf.FloorToInt(_coords.y));
        }
        public ChessFigure(ChessBoard.ChessFigure figure)
        {
            coordinates = figure.slot.Coordinates;
        }
    }
    public static ChessFigure
        blackKing,
        whiteKing;
    public static ChessFigure[]
        blackfigures = new ChessFigure[0],
        whiteFigures = new ChessFigure[0];

    public static void SaveTo(ChessBoard.GameManager manager)
    {
        List<ChessFigure>
            listBlack = new List<ChessFigure>(),
            listWhite = new List<ChessFigure>();

        blackKing = new ChessFigure(manager.BlackKing);
        whiteKing = new ChessFigure(manager.WhiteKing);

        foreach (ChessBoard.ChessFigure figure in manager.blackFigures)
        {
            listBlack.Add(new ChessFigure(figure));
        }
        foreach (ChessBoard.ChessFigure figure in manager.whiteFigures)
        {
            listWhite.Add(new ChessFigure(figure));
        }

        blackfigures = listBlack.ToArray();
        whiteFigures = listWhite.ToArray();
    }

    public static void SaveTo(Calendar.GameManager manager)
    {
        calendar = manager.calendarDict;
    }
}

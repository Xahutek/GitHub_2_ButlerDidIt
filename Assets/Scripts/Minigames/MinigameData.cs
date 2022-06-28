using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calendar;

public static class MinigameData
{
    //Calendar
    public static List<bool> calendar;

    ////Chess
    public static List<Vector2Int> slotFigures = new List<Vector2Int>();

    public static void SaveTo(ChessBoard.GameManager manager)
    {
        slotFigures = new List<Vector2Int>();

        for (int i = 0; i < manager.slots.Count; i++)
        {
            ChessBoard.BoardSlot slot = manager.slots[i];
            slotFigures.Add(new Vector2Int(i,slot.figure==null?-1:manager.figures.IndexOf(slot.figure)));
        }
    }

    public static void SaveTo(Calendar.GameManager manager)
    {
        calendar = manager.calendarDict;
    }
}

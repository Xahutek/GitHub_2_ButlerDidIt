using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Calendar
{
    public class GameManager : MinigameObject
    {
        public static GameManager main;

        public GameState gameState;
        public TMP_Text resultText;

        public Clue EdnaLastVisitor;
        public Clue TyrellLastVisitor;
        public Clue NoChange;
        public Clue TooLate;

        public CalendarDays calendar;
        public List<bool> calendarDict;
        [HideInInspector] public int daysCrossed;

        public AudioClip crossing;
        public AudioClip erasing;

        public float DetectiveSeeTime = 9;

        private void Awake()
        {
            main = this;
        }

        private void CalendarCrosses()
        {
            bool aBlock = true;
            for (int i = 0; i < calendar.allFields.Count; i++)
            {
                bool crossState = calendar.allFields[i].crossed;
                if (aBlock && crossState)
                {
                    daysCrossed = i + 1;
                    if (i + 1 > 13) { return; }
                }
                else if (!crossState)
                {
                    if (i + 1 <= 9) { return; }
                    aBlock = false;
                }
                else if (!aBlock && crossState)
                {
                    daysCrossed = 0;
                    return;
                }
            }
        }

        public void CheckState()
        {
            CalendarCrosses();
            if (daysCrossed >= 9)
            {
                if (daysCrossed == 13) { gameState = GameState.untouched; }
                else if (daysCrossed == 11) { gameState = GameState.EdnaSus; }
                else if (daysCrossed == 9) { gameState = GameState.TyrellSus; }
                else { gameState = GameState.CalendarSus; }
            }
            else { gameState = GameState.CalendarSus; }

            if(Clock.main.currentHour >= 9) 
            {
                resultText.text = "Eva already inspected this room, altering the calendar now won't change anything.";
            }
            else
            {
                switch (gameState)
                {
                    case GameState.untouched:
                        resultText.text = "How the Lord left it, a cross each day.";
                        break;
                    case GameState.EdnaSus:
                        resultText.text = "The general visited last that day.";
                        break;
                    case GameState.TyrellSus:
                        resultText.text = "The tycoon came for a visit to this date.";
                        break;
                    default: //not getting anywhere
                        resultText.text = "Who am I trying to fool with this?";
                        break;
                }
            }            
        }

        public override void Open()
        {
            base.Open();
            Load();
            CheckState();
        }
        public override void Load()
        {
            calendar.LoadCalendar(MinigameData.calendar);
        }
        public override void Close()
        {
            CheckState();
            if(Clock.main.currentHour >= 9)
            {
                if(!EdnaLastVisitor.KnownTo(Character.Butler) && !TyrellLastVisitor.KnownTo(Character.Butler))
                {
                    TooLate.MakeKnownTo(Character.Butler);
                }
            }
            else
            {
                if (gameState == GameState.EdnaSus)
                {
                    EdnaLastVisitor.MakeKnownTo(Character.Butler);
                    TyrellLastVisitor.MakeUnknownTo(Character.Butler);

                    if (Clock.Hour<DetectiveSeeTime)
                    {
                        EdnaLastVisitor.MakeKnownTo(Character.Detective);
                        TyrellLastVisitor.MakeUnknownTo(Character.Detective);
                    }
                }
                else if (gameState == GameState.TyrellSus)
                {
                    TyrellLastVisitor.MakeKnownTo(Character.Butler);
                    EdnaLastVisitor.MakeUnknownTo(Character.Butler);

                    if (Clock.Hour < DetectiveSeeTime)
                    {
                        TyrellLastVisitor.MakeKnownTo(Character.Detective);
                        EdnaLastVisitor.MakeUnknownTo(Character.Detective);
                    }
                }
                else
                {
                    NoChange.MakeKnownTo(Character.Butler);
                    TyrellLastVisitor.MakeUnknownTo(Character.Butler);
                    EdnaLastVisitor.MakeUnknownTo(Character.Butler);

                    if (Clock.Hour < DetectiveSeeTime)
                    {
                        EdnaLastVisitor.MakeUnknownTo(Character.Detective);
                        TyrellLastVisitor.MakeUnknownTo(Character.Detective);
                    }
                }
            }            
            Save();
            base.Close();
        }
        public override void Save()
        {
            calendarDict = new List<bool>();
            for (int i = 0; i < calendar.allFields.Count; i++)
            {
                calendarDict.Add(calendar.allFields[i].crossed);
            }
            MinigameData.SaveTo(this);
        }
    }

    public enum GameState
    {
        untouched, EdnaSus, TyrellSus, CalendarSus
    }
}


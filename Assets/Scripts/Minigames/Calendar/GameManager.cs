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

        public CalendarDays calendar;
        [HideInInspector]public int daysCrossed;
        private void Awake()
        {
            main = this;
        }

        private void CalendarCrosses()
        {
            bool aBlock = true;
            for (int i = 1; i < calendar.fields.Count; i++)
            {
                bool crossState = calendar.fields[i].GetComponent<DayField>().crossed;
                if (aBlock && crossState)
                {
                    daysCrossed = i;
                    if(i > 13) { return; }
                }
                else if (!crossState)
                {                    
                    if (i <= 9) { return; }
                    aBlock = false;
                }
                else if(!aBlock && crossState)
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
                if(daysCrossed== 13) { gameState = GameState.untouched; }
                else if (daysCrossed == 11) { gameState = GameState.EdnaSus; }
                else if (daysCrossed == 9) { gameState = GameState.TyrellSus; } 
                else { gameState = GameState.CalendarSus; }
            }
            else { gameState = GameState.CalendarSus; }

            switch (gameState)
            {
                case GameState.untouched:
                    resultText.text = "How the Lord left it, a cross each day.";
                    break;
                case GameState.EdnaSus:
                    resultText.text = "The General visited last that day.";
                    break;
                case GameState.TyrellSus:
                    resultText.text = "The Tycoon came for a visit to this date.";
                    break;
                default: //not getting anywhere
                    resultText.text = "Who am I trying to fool with this.";
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
            daysCrossed = MinigameData.daysCrossed;
        }
        public override void Close()
        {          
            CheckState();
            if (gameState == GameState.EdnaSus)
            {
                EdnaLastVisitor.MakeKnownTo(Character.Butler);
                TyrellLastVisitor.MakeUnknownTo(Character.Butler);
            }
            else if (gameState == GameState.TyrellSus)
            {
                TyrellLastVisitor.MakeKnownTo(Character.Butler);
                EdnaLastVisitor.MakeUnknownTo(Character.Butler);
            }
            else
            {
                TyrellLastVisitor.MakeUnknownTo(Character.Butler);
                EdnaLastVisitor.MakeUnknownTo(Character.Butler);
            }
            base.Close();
        }
        public override void Save()
        {
            MinigameData.SaveTo(this);
        }
    }

    public enum GameState
    {
        untouched, EdnaSus, TyrellSus, CalendarSus
    }
}


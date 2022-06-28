using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace ChessBoard
{
    public class BoardSlot : MonoBehaviour, IDropHandler
    {
        public ChessColor EdnaWinColor;
        public FigureType[] ValidEdnaWin;
        public ChessColor EdnaWinColorFlipped;
        public FigureType[] ValidEdnaWinFlipped;
        public ChessColor LordWinColor;
        public FigureType[] ValidLordWin;
        public ChessColor LordWinColorFlipped;
        public FigureType[] ValidLordWinFlipped;

        public ChessFigure figure;

        public bool isCrucial;
        public int siblingIndex;

        public bool isValid(ChessColor winner, bool flipped)
        {
            if (!isCrucial) return true;
            if (!figure) return false;

            ChessColor requiredColor;
            List<FigureType> AllowedTypes= new List<FigureType>();

            if (winner==ChessColor.White)
            {
                if (!flipped)
                {
                    requiredColor = EdnaWinColor;
                    AllowedTypes.AddRange(ValidEdnaWin);
                }
                else
                {
                    requiredColor = EdnaWinColorFlipped;
                    AllowedTypes.AddRange(ValidEdnaWinFlipped);
                }
            }
            else
            {
                if (!flipped)
                {
                    requiredColor = LordWinColor;
                    AllowedTypes.AddRange(ValidLordWin);
                }
                else
                {
                    requiredColor = LordWinColorFlipped;
                    AllowedTypes.AddRange(ValidLordWinFlipped);
                }
            }

            bool valid = true;

            if (figure.color != requiredColor)
                valid = false;

            if (AllowedTypes.Count != 0 && !AllowedTypes.Contains(figure.type))
                valid = false;

            return valid;
        }

        public void SetFigure(ChessFigure newFigure)
        {
            if(newFigure) newFigure.slot.figure = null;

            if (figure != null)
            {
                if (newFigure) newFigure.slot.SetFigure(figure);
                else figure.slot = null;
            }

            figure = newFigure;
            if(newFigure) figure.slot = this;

            //figure.transform.SetSiblingIndex(siblingIndex);
        }

        public void OnDrop(PointerEventData eventData)
        {
            ChessFigure F = eventData.pointerDrag.GetComponent<ChessFigure>();
            if (F != null)
            {
                SetFigure(F);

                GameManager.main.CheckState();
            }
        }
    }

}

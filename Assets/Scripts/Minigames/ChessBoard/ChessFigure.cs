using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace ChessBoard
{
    public class ChessFigure : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static ChessFigure draggedFigure;

        public ChessColor color;
        public FigureType type;

        public bool dragged;

        Image image;

        public BoardSlot slot;
        public float AdjustSpeed=10;

        private void Awake()
        {
            image=GetComponent<Image>();
        }

        bool instantAdjust;
        private void OnEnable()
        {
            instantAdjust = true;
        }

        private void FixedUpdate()
        {
            image.raycastTarget = draggedFigure == null;
            if (!dragged && slot)
            {
                Vector3 diff = slot.transform.position - transform.position;
                if (transform.position != slot.transform.position)
                {
                    if (instantAdjust || diff.magnitude <= Time.fixedDeltaTime * AdjustSpeed)
                        transform.position = slot.transform.position;
                    else
                        transform.position += diff * Time.fixedDeltaTime * AdjustSpeed;
                    instantAdjust = false;
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            dragged = true;
            draggedFigure = this;
            transform.SetAsLastSibling();
            Debug.Log("Begin Drag "+name);
        }
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            dragged=false;
            if(draggedFigure==this) draggedFigure = null;
            Debug.Log("End Drag " + name);
            GameManager.main.MakeClackNoise();
        }
    }

    public enum FigureType
    {
        King, Queen, Rook, Bishop, Knight, Pawn
    }
    public enum ChessColor
    {
        Both, Black, White
    }
}

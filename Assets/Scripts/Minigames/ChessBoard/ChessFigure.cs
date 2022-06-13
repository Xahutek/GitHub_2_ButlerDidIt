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
        public BoardSlot slot;

         bool dragged;
         Image image;

        public bool Black;
        public Type type;
        public enum Type
        {
            Pawn, King, Queen, Bishop, Knight, Rook
        }

        public BoardSlot[] GetFieldsOfInfluence()
        {
            List<BoardSlot> slots = new List<BoardSlot>();

            switch (type)
            {
                case Type.Pawn:
                    break;
                case Type.King:
                    slots.Add(slot);
                    slots.AddRange(slot.neighbours);
                    break;
                case Type.Queen:
                    break;
                case Type.Bishop:
                    slots.Add(slot);
                    for (int i = -7; i <= 7; i++)
                    {
                        Vector2
                            vec1 = new Vector2(slot.Coordinates.x + i, slot.Coordinates.y + i),
                            vec2 = new Vector2(slot.Coordinates.x - i, slot.Coordinates.y + i);

                        if (BoardSlot.slots.ContainsKey(vec1) && !slots.Contains(BoardSlot.slots[vec1]))
                            slots.Add(BoardSlot.slots[vec1]);
                        if (BoardSlot.slots.ContainsKey(vec2) && !slots.Contains(BoardSlot.slots[vec2]))
                            slots.Add(BoardSlot.slots[vec2]);
                    }
                    break;
                case Type.Knight:
                    break;
                case Type.Rook:
                    slots.Add(slot);
                    for (int i = -7; i <= 7; i++)
                    {
                        Vector2
                            vec1 = new Vector2(slot.Coordinates.x + i, slot.Coordinates.y),
                            vec2 = new Vector2(slot.Coordinates.x , slot.Coordinates.y + i);

                        if (BoardSlot.slots.ContainsKey(vec1) && !slots.Contains(BoardSlot.slots[vec1]))
                            slots.Add(BoardSlot.slots[vec1]);
                        if (BoardSlot.slots.ContainsKey(vec2) && !slots.Contains(BoardSlot.slots[vec2]))
                            slots.Add(BoardSlot.slots[vec2]);
                    }
                    break;
                default:
                    break;
            }


            return slots.ToArray();
        }

        private void Awake()
        {
            image = GetComponent<Image>();
        }
        private void FixedUpdate()
        {
            if (!dragged && slot != null)
            {
                transform.position=new Vector2(slot.transform.position.x, slot.transform.position.y);
            }
            if(draggedFigure==null)transform.SetSiblingIndex(slot.Coordinates.x+slot.Coordinates.y);

            image.raycastTarget = draggedFigure==null;

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
        }
    }
}

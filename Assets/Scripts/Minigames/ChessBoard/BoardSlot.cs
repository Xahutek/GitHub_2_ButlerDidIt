using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace ChessBoard
{
    public class BoardSlot : MonoBehaviour, IDropHandler
    {
        public static Dictionary<Vector2,BoardSlot> slots = new Dictionary<Vector2, BoardSlot>();
        [SerializeField] List<BoardSlot> allSlots;
        public BoardSlot[] neighbours;

        public Vector2Int Coordinates;
        public ChessFigure figure = null;

        private void Awake()
        {
            slots.Add(Coordinates, this);
        }
        private void Start()
        {
            allSlots = new List<BoardSlot>();
            foreach (BoardSlot s in slots.Values)
            {
                allSlots.Add(s);
            }


            List<BoardSlot> neigh = new List<BoardSlot>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector2 v = Coordinates + new Vector2(x, y);
                    if (Coordinates == Vector2Int.one * 5) Debug.DrawLine(slots[v].transform.position, slots[v].transform.position - slots[v].transform.forward * 100, Color.yellow, 5);
                    if (v == Coordinates || !slots.ContainsKey(v)) continue;
                    if (Coordinates == Vector2Int.one * 5) Debug.DrawLine(slots[v].transform.position, slots[v].transform.position - slots[v].transform.forward * 200, Color.green, 5);
                    neigh.Add(slots[v]);
                }
            }
            neighbours = neigh.ToArray();
        }

        public bool KingField(bool Black)
        {
            if (figure && figure.type == ChessFigure.Type.King && figure.Black == Black)
                return true;
            foreach (BoardSlot n in neighbours)
            {
                if (n.figure && n.figure.type == ChessFigure.Type.King && n.figure.Black == Black)
                    return true;
            }
            return false;
        }

        public void OnDrop(PointerEventData eventData)
        {
            ChessFigure F= eventData.pointerDrag.GetComponent<ChessFigure>();
            if (F != null && figure == null && !KingField(!F.Black))
            {
                F.slot.figure = null;
                F.slot = this;
                figure = F;

                GameManager.main.CheckState();
            }
        }
    }

}

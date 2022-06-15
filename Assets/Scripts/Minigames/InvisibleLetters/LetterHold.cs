using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Letters
{
    public class LetterHold : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static LetterHold main;

        public bool isHolding = false;
        Vector2 origin;

        private void Awake()
        {
            main = this;
            origin = transform.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isHolding = true;
        }
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isHolding = false;
            transform.position = origin;
        }
    }
}

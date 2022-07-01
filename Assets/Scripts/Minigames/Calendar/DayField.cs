using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace Calendar
{
    public class DayField : MonoBehaviour, IPointerClickHandler
    {

        private Collider2D coll;
        private Image image;
        public bool crossed;

        private void Awake()
        {
            coll = GetComponent<Collider2D>();
            image = GetComponent<Image>();
            if (image.color.a > 0) crossed = true;
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            SetCross(!crossed, true);
            GameManager.main.CheckState();
        }

        public void SetCross(bool cross)
        {
            crossed = cross;
            if (crossed)
            {
                image.color = Color.white;
            }
            else
            {
                image.color = Color.clear;
            }
        }
        public void SetCross(bool cross, bool byButler)
        {
            crossed = cross;
            if (crossed)
            {
                SoundManager.main.PlayOneShot(GameManager.main.crossing);
                image.color = Color.white;
            }
            else
            {
                SoundManager.main.PlayOneShot(GameManager.main.erasing);
                image.color = Color.clear;
            }
        }
    }
}

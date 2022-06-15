using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Letters
{
    public class PaperSpot : MonoBehaviour
    {
        [HideInInspector] public TMP_Text text;
        public Color inkColor;
        private Coroutine heating;
        private float time, minHoldTime = 1.5f, completeTime = 4;

        private void Awake()
        {
            text = GetComponentInChildren<TMP_Text>();
        }

        public void InkStatus()
        {
            text.color = GameManager.main.gameState == Letters.GameState.LettersObtained ? Color.clear :
                GameManager.main.gameState == Letters.GameState.Deciphered ? inkColor : text.color;
        }

        public void PaperOverFire()
        {
            if (heating == null) { heating = StartCoroutine(HeatUp()); }
            time += Time.deltaTime;
            if(GameManager.main.gameState == GameState.Deciphered && (time > (completeTime + minHoldTime)*2))
            {
                LetterHold.main.transform.GetComponentInChildren<Transform>().position += Vector3.up *2000;
                LetterHold.main.enabled = false;
                GameManager.main.gameState = GameState.Burnt;
                GameManager.main.CheckState();
            }
        }

        IEnumerator HeatUp()
        {
            while(time < minHoldTime) { yield return null; }
            while(time < completeTime + minHoldTime)
            {
                text.color = Color.Lerp(Color.clear, inkColor, EaseIn(time/(completeTime+ minHoldTime)));
                yield return null;
            }
            GameManager.main.SetPaperState();
        }

        public static float EaseIn(float t) { return  Mathf.Pow(t,3);}
    }

}

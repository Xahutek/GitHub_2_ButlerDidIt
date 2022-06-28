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
        [HideInInspector] public float time, minHoldTime = 1.5f, completeTime = 4;
        [SerializeField] Material dissolve;

        private void Awake()
        {
            text = GetComponentInChildren<TMP_Text>();
            dissolve.SetFloat("Vector1_6298484c3b4e4b1fbafd65018b3aba39", 0);
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
            while (GameManager.main.gameState == GameState.Deciphered && time >= (completeTime + minHoldTime))
            {
                //Time from 5.5 to 10.9
                //b1 + (s1-a1)*(b2-b1)/(a2-a1)
                // time, 5.5, 10.9, 0,2
                Debug.Log(0+(time-5.5f)*(2-0)/(10.9f-5.5f));
                dissolve.SetFloat("Vector1_6298484c3b4e4b1fbafd65018b3aba39", 0+(time-5.5f)*(2-0)/(12f-5.5f));
                text.color = Color.Lerp(inkColor, Color.clear, EaseIn(time / (completeTime + minHoldTime)));
                yield return null;
            }
        }

        public static float EaseIn(float t) { return  Mathf.Pow(t,3);}
    }

}

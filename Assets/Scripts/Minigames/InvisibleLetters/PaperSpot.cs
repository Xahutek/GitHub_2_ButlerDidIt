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
            dissolve.SetFloat("Vector1_6298484c3b4e4b1fbafd65018b3aba39", -0.1f);
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
        }

        IEnumerator HeatUp()
        {
            while(time < minHoldTime) { yield return null; }
            while(time < completeTime + minHoldTime)
            {
                text.color = Color.Lerp(Color.clear, inkColor, EaseIn(time/(completeTime+ minHoldTime)));
                if(GameManager.main.gameState == GameState.Burnt) { time = completeTime + minHoldTime; }
                    yield return null;
            }
            GameManager.main.SetPaperState();
            while (GameManager.main.gameState == GameState.LettersObtained) { yield return null; }
            time = completeTime + minHoldTime;
            while (dissolve.GetFloat("Vector1_6298484c3b4e4b1fbafd65018b3aba39") < 0.3f)
            {
                Dissolver(time);
                yield return null;
            }
            GameManager.main.PaperBurning();
            while(dissolve.GetFloat("Vector1_6298484c3b4e4b1fbafd65018b3aba39") >= 0.3f && dissolve.GetFloat("Vector1_6298484c3b4e4b1fbafd65018b3aba39") < 2f)
            {
                time += Time.deltaTime; 
                Dissolver(time);
                yield return null;
            }
            GameManager.main.RemoveText();
        }

        float dissolveV;
        private void Dissolver(float time)
        {
            dissolveV = -0.1f + (time - 5.5f) * (2.1f - 0.1f) / (12f - 5.5f);
            //Time from 5.5 to 10.9
            //b1 + (s1-a1)*(b2-b1)/(a2-a1)
            // time, 5.5, 10.9, 0,2    
            if(dissolveV > dissolve.GetFloat("Vector1_6298484c3b4e4b1fbafd65018b3aba39"))
            {
                dissolve.SetFloat("Vector1_6298484c3b4e4b1fbafd65018b3aba39", dissolveV);
            }
            text.color = Color.Lerp(inkColor, Color.clear, EaseIn(time / (completeTime + minHoldTime * 2)));
        }

        public static float EaseIn(float t) { return  Mathf.Pow(t,3);}
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Letters
{
    public class Fire : MonoBehaviour
    {
        [HideInInspector] public Collider2D coll;

        private void Awake()
        {
            coll = GetComponent<Collider2D>();
        }

        void Update()
        {
            //OverlapPoint mit Feuerquelle abfragen wenn das Papier gerade gehalten wird
            if (LetterHold.main.isHolding && GameManager.main.gameState != GameState.Burnt)
            {
                foreach(PaperSpot spot in GameManager.main.spots)
                {
                    if (coll.OverlapPoint(spot.transform.position))
                    {
                        spot.PaperOverFire();
                    }
                }
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photo
{
    public class GameManager : MinigameObject
    {
        public static GameManager main;

        private void Awake()
        {
            main = this;
        }

        public override void Open()
        {
            base.Open();
        }
        public override void Close()
        {
            base.Close();
        }
    }
}
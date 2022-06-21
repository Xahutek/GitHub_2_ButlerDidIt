using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Phone
{
    public class PhoneManager : MinigameObject
    {
        public void CallThePolice()
        {
            if (MinigameManager.blocked) return;

                GameManager.manualPaused = true;

            MinigameManager.main.StartCoroutine(ExecuteCallThePolice());
        }

        IEnumerator ExecuteCallThePolice()
        {
            MinigameManager.blocked = true;

            GlobalBlackscreen.multiplier = 2;
            GlobalBlackscreen.on = true;
            yield return new WaitForSeconds(0.55f);
            
            SceneManager.UnloadSceneAsync("Phone");
            yield return new WaitForSeconds(0.1f);

            MinigameManager.main.revealManager.Reveal(Character.Butler);

            MinigameManager.blocked = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager main;

    public static MinigameObject currentMinigame = null;
    public static string sceneName;
    private Coroutine execution;
    public static bool isOpen
    {
        get { return actionQueued||currentMinigame != null||SceneManager.GetSceneByName(sceneName).isLoaded; }
    }

    private void Awake()
    {
        main = this;
    }
    private void Update()
    {
        if (isOpen&&Input.GetKeyUp(KeyCode.Escape))
        {
            Close();
        }
    }

    public void Open(string name)
    {
        if (isOpen) Close();
        if(execution == null)
        execution = StartCoroutine(ExecuteAction(name,true));
    }

    public void Close()
    {
        if (!isOpen) return;

        if(currentMinigame)currentMinigame.Close();
        if (execution == null)
        {
            execution = StartCoroutine(ExecuteAction(sceneName, false));
        }
        currentMinigame = null;
    }

    static bool actionQueued = false;
    IEnumerator ExecuteAction(string name, bool open)
    {
        while (actionQueued)
        {
            yield return null;
        }
        actionQueued = true;

        GlobalBlackscreen.multiplier = 2;
        GlobalBlackscreen.on = true;
        yield return new WaitForSeconds(0.55f);

        if (open)
            SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        else
            SceneManager.UnloadSceneAsync(sceneName);
        yield return new WaitForSeconds(0.05f);

        sceneName = open ? name : "";

        GlobalBlackscreen.multiplier = 2;
        GlobalBlackscreen.on = false;
        yield return new WaitForSeconds(0.5f);
        actionQueued = false;
        execution = null;
    }

}

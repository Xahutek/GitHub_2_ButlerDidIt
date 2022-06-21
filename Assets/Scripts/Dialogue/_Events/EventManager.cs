using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(GrandRevealManager))]
public class EventManager : MonoBehaviour
{
    public static bool isOpen;
    public static EventManager main;

    public EventProfile[] allEvents;
    public Intermission[] allIntermissions;
    [System.Serializable] public class Intermission
    {
        public string message;
        public Vector2 availableTime = new Vector2(0,0f);
        public Clue gainedClue;

        public float duration
        {
            get { return availableTime.y - availableTime.x; }
        }

        public bool Triggered()
        {
            bool t = true;
            if (!Clock.HourPassed(availableTime.x))
                t = false;

            if (GameManager.isPaused)
                t = false;

            return t;
        }
    }

    GrandRevealManager revealManager;
    public EventDialogueManager dialogue;
    public EventProfile currentProfile;
    public EventObject room;

    private void Awake()
    {
        main = this;
    }
    private void Start()
    {
        revealManager= GetComponent<GrandRevealManager>();
    }

    private void Update()
    {
        if (!isOpen&&!GameManager.isPaused)
        {
            if (Clock.HourPassed(24f))
            {
                revealManager.Reveal();
                return;
            }
            foreach (EventProfile E in allEvents)
            {
                if (E.Triggered())
                {
                    StartEvent(E);
                    return;
                }
            }
            foreach (Intermission I in allIntermissions)
            {
                if (I.Triggered())
                {
                    StartIntermission(I);
                    return;
                }
            }
        }
    }

    public void StartIntermission(Intermission inter)
    {
        Debug.Log("Start Intermission");

        if (EventRoutine != null) return;

        isOpen = true;
        if (IntermissionRoutine!=null) StopCoroutine(IntermissionRoutine);
        EventRoutine = StartCoroutine(IntermissionLoop(inter));
    }
    public void StartEvent(EventProfile profile)
    {
        Debug.Log("Start Event in "+profile.SceneName);

        isOpen = true;
        this.currentProfile = profile;
        if (EventRoutine != null) StopCoroutine(EventRoutine);
        EventRoutine = StartCoroutine(EventLoop());
    }

    Coroutine EventRoutine=null, IntermissionRoutine;
    IEnumerator EventLoop()
    {
        isOpen = true;

        //Load Scene and set it up
        GlobalBlackscreen.multiplier = 1;
        GlobalBlackscreen.on = true;

        float
            hours = Mathf.FloorToInt(currentProfile.availableTime.x),
            minutes = (currentProfile.availableTime.x - hours) * 60;

        string timeText = string.Format("{0:00}:{1:00}", hours, minutes) + " \n ";

        GlobalBlackscreen.message =
            "<color=#808080ff>" + timeText+ "</color>"
            + currentProfile.introduction;
        yield return new WaitForSeconds(1);

        room = null;
        string SceneName = currentProfile.SceneName;

        bool isLoaded = SceneManager.GetSceneByName(SceneName).isLoaded;
        if (isLoaded)
        {
            SceneManager.UnloadSceneAsync(SceneName);
            yield return null;
        } //Unload for reset
        SceneManager.LoadSceneAsync(currentProfile.SceneName, LoadSceneMode.Additive);
        while (room==null)
        {
            yield return null;
        } //Wait for scene to load correctly

        while (true)
        {
            if (Input.anyKeyDown) break;
            yield return null;
        }//Wait for intro skip

        GlobalBlackscreen.multiplier = 1;
        GlobalBlackscreen.on = false;
        yield return new WaitForSeconds(1);

        //Start Dialogue
        dialogue.profile = currentProfile;
        dialogue.Open(room.eventCharacters, currentProfile.StartDialogue);

        while (DialogueManager.isOpen)
        {
            yield return null;
        }

        //End Event

        yield return new WaitForSeconds(0.1f);

        //while (true) //Just a manual leave thing
        //{
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        break;
        //    }
        //    yield return null;
        //}

        Clock.PassHours(currentProfile.duration);
        room.RespawnPlayer();

        currentProfile = null;
        room = null;

        yield return new WaitForSeconds(0.1f);
        isOpen = false;

        Debug.Log("End Event");
    }

    IEnumerator IntermissionLoop(Intermission inter)
    {
        isOpen = true;

        GlobalBlackscreen.multiplier = 2;
        GlobalBlackscreen.on = true;

        float
            hours = Mathf.FloorToInt(inter.availableTime.x),
            minutes = (inter.availableTime.x - hours) * 60;

        string timeText = string.Format("{0:00}:{1:00}", hours, minutes) + " \n ";

        GlobalBlackscreen.message =
            "<color=#808080ff>" + timeText + "</color>"
            + inter.message;

        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            if (Input.anyKeyDown) break;
            yield return null;
        }//Wait for intro skip

        Clock.PassHours(inter.duration);

        GlobalBlackscreen.multiplier = 2;
        GlobalBlackscreen.on = false;
        yield return new WaitForSeconds(0.35f);

        if(inter.gainedClue!=null)
        inter.gainedClue.MakeKnownTo(Character.Butler);
        isOpen = false;
    }
}

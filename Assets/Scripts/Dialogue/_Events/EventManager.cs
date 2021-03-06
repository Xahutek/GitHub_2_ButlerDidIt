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
    public static bool blockRoomRefreshs;
    public AudioClip hourlyClock;
    public bool saveFileLoaded = false;
    [System.Serializable] public class Intermission
    {
        [TextArea] public string message;
        public Vector2 availableTime = new Vector2(0,0f);
        public Clue gainedClue;
        public AudioClip[] sfx;
        public bool passed;

        public float duration
        {
            get { return availableTime.y - availableTime.x; }
        }

        public bool Triggered(out bool soon)
        {
            bool t = true;
            if (!Clock.HourPassed(availableTime.x))
                t = false;

            if (GameManager.isPaused)
                t = false;

            if (passed) t = false;

            soon = Clock.HourPassed(availableTime.x - (5 / 60));

            return t;
        }
    }

    GrandRevealManager revealManager;
    public EventDialogueManager dialogue;
    public EventProfile currentProfile;
    public EventObject room;
    public static bool eventSoon;
    private float everyHour;
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
        eventSoon = false;
        if (!isOpen && !GameManager.isPaused && saveFileLoaded)
        {        
            if(everyHour == 0) { everyHour = Mathf.Ceil(Clock.TotalHours); Debug.Log(everyHour); }
            if (Clock.Hour >= 24f)
            {
                Debug.Log("Revealing");
                revealManager.Reveal();
                return;
            }
            if (Clock.HourPassed(everyHour))
            {                
                SoundManager.main.PlayOneShot(hourlyClock);
                everyHour++;
            }
            foreach (EventProfile E in allEvents)
            {
                if (E.Triggered(out eventSoon))
                {
                    StartEvent(E);
                    return;
                }
            }
            foreach (Intermission I in allIntermissions)
            {
                if (I.Triggered(out eventSoon))
                {
                    Debug.Log("Started Intermission: "+I.message);
                    StartIntermission(I);
                    return;
                }
            }
        }
    }
    //public void CheckFirstIntemrission()
    //{
    //    Intermission I = SpecialFirstIntermission;
    //    if (I.Triggered(out eventSoon))
    //    {
    //        Debug.Log("Started Intermission: " + I.message);
    //        StartIntermission(I);
    //    }
    //    else
    //        GlobalBlackscreen.on = false;
    //}

    public void StartIntermission(Intermission inter)
    {
        Debug.Log("Start Intermission");

        if (IntermissionRoutine != null) return;
        DialogueManager.main.Close();

        isOpen = true;
        if (IntermissionRoutine!=null) StopCoroutine(IntermissionRoutine);
        IntermissionRoutine = StartCoroutine(IntermissionLoop(inter));
    }
    public void StartEvent(EventProfile profile)
    {
        Debug.Log("Start Event in "+profile.SceneName);

        DialogueManager.main.Close();

        isOpen = true;
        this.currentProfile = profile;
        if (EventRoutine != null) StopCoroutine(EventRoutine);
        EventRoutine = StartCoroutine(EventLoop());
        SoundManager.main.OnEventStart(profile);
    }

    Coroutine EventRoutine=null, IntermissionRoutine=null;
    IEnumerator EventLoop()
    {
        isOpen = true;
        blockRoomRefreshs = true;

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
        GlobalBlackscreen.on = true;

        yield return new WaitForSeconds(0.3f);
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
        GlobalBlackscreen.multiplier = 1;
        GlobalBlackscreen.on = true;
        yield return new WaitForSeconds(1);

        Clock.SetTime(currentProfile.availableTime.y);
        room.RespawnPlayer();
        room.DeactivateCharacters();

        currentProfile = null;
        room = null;

        isOpen = false;
        blockRoomRefreshs = false;
        EventSystem.main.RefreshRooms();

        GlobalBlackscreen.multiplier = 1;
        GlobalBlackscreen.on = false;
        yield return new WaitForSeconds(1);
        Debug.Log("End Event");
        SoundManager.main.OnEventStop();
        EventRoutine = null;
    }

    IEnumerator IntermissionLoop(Intermission inter)
    {
        if (inter.sfx.Length > 0)
        {
            foreach(AudioClip clip in inter.sfx)
            {
                SoundManager.main.PlayOneShot(clip);
            }
        }

            isOpen = true;
        blockRoomRefreshs = false;

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

        EventSystem.main.RefreshRooms();
        GlobalBlackscreen.on = true;

        yield return new WaitForSeconds(0.3f);
        while (true)
        {
            if (Input.anyKeyDown&&Time.timeSinceLevelLoad>1) break;
            yield return null;
        }//Wait for intro skip

        Clock.SetTime(inter.availableTime.y);

        GlobalBlackscreen.multiplier = 2;
        GlobalBlackscreen.on = false;
        yield return new WaitForSeconds(0.5f);

        if(inter.gainedClue!=null)
        inter.gainedClue.MakeKnownTo(Character.Butler);

        inter.passed = true;

        yield return new WaitForSeconds(0.1f);

        isOpen = false;
        blockRoomRefreshs = false;
        IntermissionRoutine = null;
    }
}

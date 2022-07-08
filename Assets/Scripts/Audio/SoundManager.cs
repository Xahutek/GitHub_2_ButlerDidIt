using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager main;

    [SerializeField]
    private AudioSource effectSource;
     SoundSource[] sources;

    public float _volume = 1;
    [HideInInspector] public float volume = 0;

    private void Awake()
    {
        main = this;
    }
    private void Start()
    {
        EventSystem events = EventSystem.main;
        if(events)events.OnChangeRoom += OnChangeRoom;

        List<SoundSource> sourceCollect = new List<SoundSource>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            sourceCollect.AddRange(child.GetComponentsInChildren<SoundSource>());
        }
        sources=sourceCollect.ToArray();

        RefreshVolume();
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR
        if (volume != _volume)
        {
            PlayerPrefs.SetFloat("Master_Volume", _volume);
            RefreshVolume();
        }
#endif
    }
    public void RefreshVolume()
    {
        volume = PlayerPrefs.GetFloat("Master_Volume");
        _volume = volume;

        effectSource.volume = volume;
    }

    #region MusicControl

    [SerializeField]private Room r;
    [SerializeField]private Character c;
    [SerializeField]private EventProfile e;
    public void Refresh()
    {
        foreach (SoundSource S in sources)
        {
            S.Refresh(r,c,e);
        }
    }
    public void OnSpeakCharacter(Character character)
    {
        c = character;
        Refresh();
    }
    public void OnChangeRoom(Room room, Character character)
    {
        if (character != Character.Butler) return;

        r = room;
        Refresh();
    }

    public void OnEventStart(EventProfile profile)
    {
        e = profile;
        Refresh();
    }
    public void OnEventStop()
    {
        e = null;
        Refresh();
    }

    public void PlayOneShot(AudioClip clip, bool randomPitch)
    {
        effectSource.pitch = Random.Range(0.7f, 1.3f);
        effectSource.PlayOneShot(clip);
    }
    public void PlayOneShot(AudioClip clip)
    {
        effectSource.pitch = 1;
        effectSource.PlayOneShot(clip);
    }

    #endregion
}

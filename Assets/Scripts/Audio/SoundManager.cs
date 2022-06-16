using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager main;

    [SerializeField]
    public AudioSource effectSource;
     SoundSource[] sources;

    public float _volume = 1;
    private float volume = 0;

    private void Awake()
    {
        main = this;
    }
    private void Start()
    {
        EventSystem events = EventSystem.main;
        events.OnChangeRoom += OnChangeRoom;

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

        effectSource.volume = volume;
    }

    #region MusicControl

    [SerializeField]private Room r;
    [SerializeField]private Character c;
    public void Refresh()
    {
        foreach (SoundSource S in sources)
        {
            S.Refresh(volume,r,c);
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

    #endregion
}

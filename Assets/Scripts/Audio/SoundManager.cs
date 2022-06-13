using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager main;

    [SerializeField] public AudioSource effectSource, 
        BackgroundMusicSource0, BackgroundMusicSource1,
        BackgroundAmbienceSource0, BackgroundAmbienceSource1;

    bool _isFirst = true;

    AudioSource activeSourceMusic
    {
        get { return _isFirst ? BackgroundMusicSource0 : BackgroundMusicSource1; }
        set
        {
            _isFirst = value == BackgroundMusicSource0;
        }
    }
    AudioSource activeSourceAmbient
    {
        get { return _isFirst ? BackgroundMusicSource0 : BackgroundMusicSource1; }
        set
        {
            _isFirst = value == BackgroundMusicSource0;
        }
    }

    Coroutine _zerothSourceFadeRoutineMusic = null, _zerothSourceFadeRoutineAmbience = null;
    Coroutine _firstSourceFadeRoutineMusic = null, _firstSourceFadeRoutineAmbience = null;

    public float fadeDuration = 1.5f, _volume = 1;
    private float volume=0;

    private void Awake()
    {
        main = this;
    }
    private void Start()
    {
        EventSystem events = EventSystem.main;
        events.OnChangeRoom += OnChangeRoom;

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
        BackgroundAmbienceSource0.volume = volume;
        BackgroundAmbienceSource1.volume = volume;
        BackgroundMusicSource0.volume = volume;
        BackgroundMusicSource1.volume = volume;

        //Add all future sources
    }

    #region RoomMusic

    [System.Serializable]public class RoomAudio
    {
        public Room room;
        public AudioClip
            music,
            ambience;
    }

    public RoomAudio[] RoomAudios;

    public void OnChangeRoom(Room room, Character character)
    {
        if (character != Character.Butler) return;

        foreach (RoomAudio R in RoomAudios)
        {
            if(R.room==room)
            {
                Debug.Log("Play music of "+room.ToString());
                if (R.music != null) CrossFadeBackgroundMusic(R.music);
                if (R.ambience != null) CrossFadeBackgroundAmbience(R.ambience);
            }
        }
    }

    #endregion

    #region CROSSFADE

    //gradually shifts the sound comming from our audio sources to the this clip:
    // maxVolume should be in 0-to-1 range
    public void CrossFadeBackgroundMusic(AudioClip playMe, float delay_before_crossFade = 0)
    {
        if (activeSourceMusic.clip == playMe)
            return;
        var fadeRoutine = StartCoroutine(FadeBackgroundMusic(playMe,
                                                    volume,
                                                    fadeDuration,
                                                    delay_before_crossFade));
    }
    public void CrossFadeBackgroundAmbience(AudioClip playMe, float delay_before_crossFade = 0)
    {
        if (activeSourceAmbient.clip == playMe)
            return;
        var fadeRoutine = StartCoroutine(FadeBackgroundMusic(playMe,
                                                    volume,
                                                    fadeDuration,
                                                    delay_before_crossFade));
    }

    IEnumerator FadeBackgroundMusic(AudioClip playMe, float maxVolume, float fadingTime, float delay_before_crossFade = 0)
    {
        if (delay_before_crossFade > 0)
        {
            yield return new WaitForSeconds(delay_before_crossFade);
        }

        if (_isFirst)
        { // _source0 is currently playing the most recent AudioClip
            //so launch on source1
            BackgroundMusicSource1.clip = playMe;
            BackgroundMusicSource1.Play();
            BackgroundMusicSource1.volume = 0;

            if (_firstSourceFadeRoutineMusic != null)
            {
                StopCoroutine(_firstSourceFadeRoutineMusic);
            }
            _firstSourceFadeRoutineMusic = StartCoroutine(fadeSource(BackgroundMusicSource1,
                                                                BackgroundMusicSource1.volume,
                                                                maxVolume,
                                                                fadingTime));
            if (_zerothSourceFadeRoutineMusic != null)
            {
                StopCoroutine(_zerothSourceFadeRoutineMusic);
            }
            _zerothSourceFadeRoutineMusic = StartCoroutine(fadeSource(BackgroundMusicSource0,
                                                                 BackgroundMusicSource0.volume,
                                                                 0,
                                                                 fadingTime));
            _isFirst = false;

            yield break;
        }

        //otherwise, _source1 is currently active, so play on _source0
        BackgroundMusicSource0.clip = playMe;
        BackgroundMusicSource0.Play();
        BackgroundMusicSource0.volume = 0;

        if (_zerothSourceFadeRoutineMusic != null)
        {
            StopCoroutine(_zerothSourceFadeRoutineMusic);
        }
        _zerothSourceFadeRoutineMusic = StartCoroutine(fadeSource(BackgroundMusicSource0,
                                                            BackgroundMusicSource0.volume,
                                                            maxVolume,
                                                            fadingTime));

        if (_firstSourceFadeRoutineMusic != null)
        {
            StopCoroutine(_firstSourceFadeRoutineMusic);
        }
        _firstSourceFadeRoutineMusic = StartCoroutine(fadeSource(BackgroundMusicSource1,
                                                            BackgroundMusicSource1.volume,
                                                            0,
                                                            fadingTime));
        _isFirst = true;
    }
    IEnumerator FadeBackgroundAmbience(AudioClip playMe, float maxVolume, float fadingTime, float delay_before_crossFade = 0)
    {
        if (delay_before_crossFade > 0)
        {
            yield return new WaitForSeconds(delay_before_crossFade);
        }

        if (_isFirst)
        { // _source0 is currently playing the most recent AudioClip
            //so launch on source1
            BackgroundAmbienceSource1.clip = playMe;
            BackgroundAmbienceSource1.Play();
            BackgroundAmbienceSource1.volume = 0;

            if (_firstSourceFadeRoutineAmbience != null)
            {
                StopCoroutine(_firstSourceFadeRoutineAmbience);
            }
            _firstSourceFadeRoutineAmbience = StartCoroutine(fadeSource(BackgroundAmbienceSource1,
                                                                BackgroundAmbienceSource1.volume,
                                                                maxVolume,
                                                                fadingTime));
            if (_zerothSourceFadeRoutineAmbience != null)
            {
                StopCoroutine(_zerothSourceFadeRoutineAmbience);
            }
            _zerothSourceFadeRoutineAmbience = StartCoroutine(fadeSource(BackgroundAmbienceSource0,
                                                                 BackgroundAmbienceSource0.volume,
                                                                 0,
                                                                 fadingTime));
            _isFirst = false;

            yield break;
        }

        //otherwise, _source1 is currently active, so play on _source0
        BackgroundAmbienceSource0.clip = playMe;
        BackgroundAmbienceSource0.Play();
        BackgroundAmbienceSource0.volume = 0;

        if (_zerothSourceFadeRoutineAmbience != null)
        {
            StopCoroutine(_zerothSourceFadeRoutineAmbience);
        }
        _zerothSourceFadeRoutineAmbience = StartCoroutine(fadeSource(BackgroundAmbienceSource0,
                                                            BackgroundAmbienceSource0.volume,
                                                            maxVolume,
                                                            fadingTime));

        if (_firstSourceFadeRoutineAmbience != null)
        {
            StopCoroutine(_firstSourceFadeRoutineAmbience);
        }
        _firstSourceFadeRoutineAmbience = StartCoroutine(fadeSource(BackgroundAmbienceSource1,
                                                            BackgroundAmbienceSource1.volume,
                                                            0,
                                                            fadingTime));
        _isFirst = true;
    }

    IEnumerator fadeSource(AudioSource sourceToFade, float startVolume, float endVolume, float duration)
    {
        float startTime = Time.time;

        while (true)
        {

            if (duration == 0)
            {
                sourceToFade.volume = endVolume;
                break;//break, to prevent division by  zero
            }
            float elapsed = Time.time - startTime;

            sourceToFade.volume = Mathf.Clamp01(Mathf.Lerp(startVolume,
                                                            endVolume,
                                                            elapsed / duration));

            if (sourceToFade.volume == endVolume)
            {
                break;
            }
            yield return null;
        }//end while
    }


    //returns false if BOTH sources are not playing and there are no sounds are staged to be played.
    //also returns false if one of the sources is not yet initialized
    public bool isPlaying
    {
        get
        {
            if (BackgroundMusicSource0 == null || BackgroundMusicSource1 == null)
            {
                return false;
            }

            //otherwise, both sources are initialized. See if any is playing:
            if (BackgroundMusicSource0.isPlaying || BackgroundMusicSource1.isPlaying)
            {
                return true;
            }

            //none is playing:
            return false;
        }//end get
    }

    #endregion
}

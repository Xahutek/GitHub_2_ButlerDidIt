using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSource : MonoBehaviour
{
    SoundManager soundManager;
    public List<Character> character;
    public List<Room> room;

    public AudioClip clip;
    AudioSource source;

    public bool active;
    public float fadeSpeed=1;
    private float volume
    {
        get { return soundManager.volume; }
    }

    private void Awake()
    {
        soundManager= FindObjectOfType<SoundManager>();
        source = GetComponent<AudioSource>();
    }
    private void Start()
    {
        source.clip = clip;
        source.Play();
        source.volume = 0;
    }

    private void Update()
    {
        float targetVolume = Mathf.Clamp(active ? 1 : 0, 0, volume);

        if (source.volume!=targetVolume)
        {
            float diff=targetVolume - source.volume;
            source.volume = Mathf.Clamp(
                source.volume + Mathf.Sign(diff) * Time.deltaTime * fadeSpeed, 
                0, 
                volume);
        }
    }

    public void Refresh(Room r, Character c)
    {
        active = room.Contains(r) || character.Contains(c);
    }

    //IEnumerator fadeSource(AudioSource sourceToFade, float startVolume, float endVolume, float duration)
    //{
    //    float startTime = Time.time;

    //    while (true)
    //    {

    //        if (duration == 0)
    //        {
    //            sourceToFade.volume = endVolume;
    //            break;//break, to prevent division by  zero
    //        }
    //        float elapsed = Time.time - startTime;

    //        sourceToFade.volume = Mathf.Clamp01(Mathf.Lerp(startVolume,
    //                                                        endVolume,
    //                                                        elapsed / duration));

    //        if (sourceToFade.volume == endVolume)
    //        {
    //            break;
    //        }
    //        yield return null;
    //    }//end while

    //    sourceToFade.volume = endVolume;
    //    if (endVolume == 0)
    //        sourceToFade.clip = null;
    //}
}

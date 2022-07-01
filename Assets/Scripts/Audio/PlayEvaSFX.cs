using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEvaSFX : MonoBehaviour
{
    public AudioClip clack;
    public void ClackEffect()
    {
        SoundManager.main.PlayOneShot(clack);
    }

}

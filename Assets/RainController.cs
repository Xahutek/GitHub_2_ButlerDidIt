using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainController : MonoBehaviour
{
    ParticleSystem rain;
    Clock clocK;
    // Start is called before the first frame update
    void Start()
    {
        rain = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Clock.Hour > 6)
        {
            var emission = rain.emission;
            emission.rateOverTime = 0;
        }
    }
}

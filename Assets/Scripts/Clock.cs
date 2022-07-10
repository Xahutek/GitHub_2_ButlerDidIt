using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public static Clock main;
    public static float TotalHours
    {
        get
        {
            return ((Hour * 10) + (Minute / 60)) / 10;
        }
    }
    public static float TotalMinutes
    {
        get
        {
            return (Hour * 60) + Minute;
        }
    }
    public static float Hour, Minute;

    public static float deltaTime, fixedDeltaTime;

    static bool internalRefresh;
    public static void SetTime(float time)
    {
        Hour = time;
        Minute = time * 60;
        LastHour = Hour-0.01f;

        internalRefresh = true;
    }
    public static void PassHours(float duration)
    {
        Hour += duration;
        Minute += duration * 60;
        LastHour = Hour-0.01f;

        internalRefresh = true;
    }

    static float LastHour = 0;
    public static bool HourPassed(float hour)
    {
        return LastHour<hour&&hour<=Hour;
    }

    private float lastCurrentHour=0;
    public float currentHour, currentMinute, CompleteDay = 60, SpeedUpMultiplier;

    private void Awake()
    {
        main = this;

        LastHour = currentHour-(1/60f);
        lastCurrentHour = currentHour;
        Hour = currentHour;
        Minute = currentMinute;
    }
    private void Update()
    {
        if (GameManager.isPaused)
        {
            deltaTime = 0;
            fixedDeltaTime = 0;
            return;
        }

        float dayFactor = CompleteDay;
        if (PlayerController.isLicking)
            dayFactor = CompleteDay/SpeedUpMultiplier;

        if (internalRefresh)
        {
            internalRefresh = false;
            currentHour = Hour;
            currentMinute = currentHour * 60;
        }

        if (lastCurrentHour != currentHour)//On ExternalChange
            LastHour = currentHour - (1 / 60f);
        else
            LastHour = currentHour;

        float
            passingTime = Time.deltaTime,
            timefactor = 24f / dayFactor,
            normalized = passingTime * timefactor;

        currentMinute += normalized;
        currentHour += normalized / 60;


        Hour = currentHour;
        Minute = currentMinute;

        lastCurrentHour = currentHour;

        deltaTime = Time.deltaTime;
        fixedDeltaTime = Time.fixedDeltaTime;
    }
}

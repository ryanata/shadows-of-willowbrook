using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DayLightCycle : MonoBehaviour
{
    public float dayIntensity = 1f;
    public float nightIntensity = 0.1f;
    public float dayDuration = 180f; // 3 minutes
    public float nightDuration = 180f; // 3 minutes
    public float transitionDuration = 60f; // 1 minute

    private UnityEngine.Rendering.Universal.Light2D globalLight;
    private float cycleDuration;

    void Start()
    {
        globalLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        cycleDuration = dayDuration + transitionDuration + nightDuration + transitionDuration;
    }

    void Update()
    {
        float time = Time.time % cycleDuration;

        if (time < dayDuration)
        {
            globalLight.intensity = dayIntensity;
        }
        else if (time < dayDuration + transitionDuration)
        {
            float t = (time - dayDuration) / transitionDuration;
            globalLight.intensity = Mathf.Lerp(dayIntensity, nightIntensity, t);
        }
        else if (time < dayDuration + transitionDuration + nightDuration)
        {
            globalLight.intensity = nightIntensity;
        }
        else
        {
            float t = (time - dayDuration - transitionDuration - nightDuration) / transitionDuration;
            globalLight.intensity = Mathf.Lerp(nightIntensity, dayIntensity, t);
        }
    }
}


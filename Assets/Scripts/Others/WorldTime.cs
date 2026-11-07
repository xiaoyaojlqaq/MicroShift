using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldTime : MonoBehaviour
{
    Light2D light;
    public float timeScale;
    public static bool isDaytime;

    public float lastTime;
    float timer;

    public TimeChangeType currTimeChangeType;
    public enum TimeChangeType
    {
        Increase,
        Reduce,
        Sustain,
    }
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();
        isDaytime = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (light.intensity >= 1)
        {
            light.intensity = 1;
            currTimeChangeType = TimeChangeType.Sustain;
            if (currTimeChangeType == TimeChangeType.Sustain)
            {
                if (timer < lastTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    timer = 0;
                    currTimeChangeType = TimeChangeType.Reduce;
                }
            }
        }
        else if (light.intensity <= 0.2f)
        {
            light.intensity = 0.2f;
            currTimeChangeType = TimeChangeType.Sustain;
            if (currTimeChangeType == TimeChangeType.Sustain)
            {
                if (timer < lastTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    timer = 0;
                    currTimeChangeType = TimeChangeType.Increase;
                }
            }

        }



        if (currTimeChangeType == TimeChangeType.Reduce) light.intensity -= Time.deltaTime * timeScale;
        else if (currTimeChangeType == TimeChangeType.Increase) light.intensity += Time.deltaTime * timeScale;

        if (light.intensity >= 0.4f) isDaytime = true;
        else isDaytime = false;
    }
}

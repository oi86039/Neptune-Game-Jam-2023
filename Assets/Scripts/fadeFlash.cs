using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class fadeFlash : MonoBehaviour
{
    Light2D myLight;
    Light light2;
    float timer = 0;
    public float startFade = 1;

    void Start()
    {
        myLight = GetComponent<Light2D>();
        light2 = GetComponent<Light>();
        startFade += Time.time;
    }

    void Update()
    {
        if (Time.time > startFade)
        {
            myLight.intensity -= Time.deltaTime;
            light2.intensity -= (Time.deltaTime * 10);
        }
        if(myLight.intensity <= 0)
        {

            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class fadeFlash : MonoBehaviour
{
    Light2D myLight;
    public float startFade = 1;

    void Start()
    {
        myLight = GetComponent<Light2D>();
        startFade += Time.time;
    }

    void Update()
    {
        if (Time.time > startFade)
        {
            myLight.intensity -= Time.deltaTime;
        }
        if(myLight.intensity <= 0)
        {

            Destroy(gameObject);
        }
    }
}

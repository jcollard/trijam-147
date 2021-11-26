using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    
    private Light Light;
    public float minIntensity = 1;
    public float maxIntensity = 1.5f;
    public float speed = 2;

    void Start()
    {
        this.Light = this.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        this.Light.intensity = (((Mathf.Sin(Time.time * speed) + 1))/2) * (maxIntensity - minIntensity) + minIntensity;
        // this.Light.intensity = (Mathf.Sin(Time.time) + 1)/2;
    }
}

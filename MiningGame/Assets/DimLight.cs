using UnityEngine;
using System.Collections;

public class DimLight : MonoBehaviour
{

    public Light mainLightSource;

    public float interpolationTime;
    public float dimmedIntensity;
    float baseIntensity;
    

    bool interpolating = false;
    float startTime = -100f;
    float dimDirection = -1f;

    // Use this for initialization
    void Start()
    {
        baseIntensity = mainLightSource.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (interpolating)
        {
            InterpolateIntensity();
        }
    }

    public void DimLights()
    {
        startTime = Time.time;
        interpolating = true;
        dimDirection = -1f;
    }

    public void EndDim()
    {
        startTime = Time.time;
        interpolating = true;
        dimDirection = 1f;
    }

    void EndInterpolation()
    {
        interpolating = false;
    }

    void InterpolateIntensity()
    {
        float timeElapsed = Time.time - startTime;
        float frac = timeElapsed / interpolationTime;
        if (frac >= 1f)
        {
            EndInterpolation();
            Destroy(gameObject);
            return;
        }
        if (dimDirection < 0)
        {
            mainLightSource.intensity = Mathf.Lerp(baseIntensity, dimmedIntensity, frac);
        }
        else
        {
            mainLightSource.intensity = Mathf.Lerp(dimmedIntensity, baseIntensity, frac);
        }

    }
}

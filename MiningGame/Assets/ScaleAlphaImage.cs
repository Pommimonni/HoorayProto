using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ScaleAlphaImage : MonoBehaviour
{
    Image myImage;
    bool scaling = false;
    public float scaleDuration = 2f;
    public float endAlpha = 0.5f;
    public float startAlpha = 0f;
    float startTime = 0;
    float direction = 1;
    public Color changedColor;
    public float newAlpha = 1f;

    // Use this for initialization
    void Awake()
    {
        myImage = this.GetComponent<Image>();
        startTime = Time.time;
        //  startAlpha = myText.color.a;
        scaling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (myImage)
        {
            float elapsedTime = Time.time - startTime;
            if (Time.time - startTime < scaleDuration)
            {
                if (direction < 0)
                    newAlpha = Mathf.Lerp(endAlpha, startAlpha, (Time.time - startTime) / scaleDuration);
                else
                    newAlpha = Mathf.Lerp(startAlpha, endAlpha, (Time.time - startTime) / scaleDuration);
                Color currectColor = myImage.color;
                myImage.color = new Color(currectColor.r, currectColor.g, currectColor.b, newAlpha);
            }
            else
            {
                startTime = Time.time;
                direction *= -1;
            }
        }
        else
        {
            myImage = this.GetComponent<Image>();
        }
    }
}

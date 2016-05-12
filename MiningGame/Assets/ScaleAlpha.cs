using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ScaleAlpha : MonoBehaviour {

    Text myText;
    bool scaling = false;
    public float scaleDuration = 2f;
    public float endAlpha = 0.5f;
    public float startAlpha = 0f;
    float startTime = 0;
    float direction = 1;
    public Color changedColor;
    public float newAlpha = 1f;

	// Use this for initialization
	void Awake () {
        myText = this.GetComponent<Text>();
        startTime = Time.time;
      //  startAlpha = myText.color.a;
        scaling = true;
	}
	
	// Update is called once per frame
	void Update () {
        float elapsedTime = Time.time - startTime;
        if (Time.time - startTime < scaleDuration)
        {
            if(direction<0)
                newAlpha = Mathf.Lerp(endAlpha, startAlpha, (Time.time - startTime) / scaleDuration);
            else
                newAlpha = Mathf.Lerp(startAlpha,endAlpha, (Time.time - startTime) / scaleDuration);
            Color currectColor = myText.color;
            myText.color = new Color(currectColor.r, currectColor.g, currectColor.b, newAlpha);
        }
        else
        {
            startTime = Time.time;
            direction *= -1;
        }
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeColorBasedOnToggle : MonoBehaviour {

	// Use this for initialization
	void Start () {
        myText = this.GetComponent<Text>();
	}
    Text myText;
    public Toggle toggleToFollow;
    public Color falseColor = Color.white;
    //public Color trueColor = Color.red;
	// Update is called once per frame
	void Update () {
        if (toggleToFollow.isOn)
        {
            myText.color = Common.defaultRedTextColor;
        }
        else
        {
            myText.color = falseColor;
        }
	}
}

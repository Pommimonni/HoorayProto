using UnityEngine;
using System.Collections;

public class DefaultColour : MonoBehaviour {
    public Color defaultRed =Color.red;
	// Use this for initialization
	void Awake () {
        Common.defaultRedTextColor = this.defaultRed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

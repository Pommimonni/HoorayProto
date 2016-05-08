using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SetDefaultRed : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (Common.defaultRedTextColor != Color.white)
        {
            GetComponent<Text>().color = Common.defaultRedTextColor;
        }
	}

    void OnEnable()
    {
        if (Common.defaultRedTextColor != Color.white)
        {
            GetComponent<Text>().color = Common.defaultRedTextColor;
        }
    }


	

}

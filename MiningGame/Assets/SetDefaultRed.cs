using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SetDefaultRed : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Text>().color = Common.defaultRedTextColor;
	}

    void OnEnable()
    {
        GetComponent<Text>().color = Common.defaultRedTextColor;
    }


	

}

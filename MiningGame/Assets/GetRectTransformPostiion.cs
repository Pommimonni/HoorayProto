using UnityEngine;
using System.Collections;

public class GetRectTransformPostiion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public Vector3 position;
	// Update is called once per frame
	void Update () {
        position=this.GetComponent<RectTransform>().position;
	}
}

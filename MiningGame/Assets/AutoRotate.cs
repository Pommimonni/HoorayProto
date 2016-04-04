using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour {
    Transform lastParent;
    float speed = 0f;
	// Use this for initialization
	void Start () {
        lastParent = this.gameObject.transform.parent;
    }
    public Vector3 rotationDirection;
    public float rotationAmount=0.1f;
	// Update is called once per frame
	void Update () {
        lastParent.Rotate(rotationAmount * rotationDirection);
    }
}

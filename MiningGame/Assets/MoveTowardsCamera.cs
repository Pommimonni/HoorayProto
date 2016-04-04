using UnityEngine;
using System.Collections;

public class MoveTowardsCamera : MonoBehaviour {

    // Use this for initialization
    Vector3 cameraPosition;
	void Start () {
        cameraPosition = Camera.main.transform.position;
        lastParent = this.gameObject.transform.parent;
    }
    Transform lastParent;
    public float speed = 0.05f;
	// Update is called once per frame
	void Update () {
        lastParent.transform.position = Vector3.MoveTowards(lastParent.transform.position, cameraPosition, speed);

    }
}

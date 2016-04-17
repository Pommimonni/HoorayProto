using UnityEngine;
using System.Collections;

public class LookAtGoingDirection : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    public Rigidbody toFollow;
	void Update () {

            transform.position = toFollow.transform.position;
            transform.LookAt(transform.position + toFollow.velocity);

    }
   
}

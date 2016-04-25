using UnityEngine;
using System.Collections;

public class TorqueAddTest : MonoBehaviour {
    public Vector3 torque;
	// Use this for initialization
	void Start () {
        TorqueAdding(this.transform.gameObject);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void TorqueAdding(GameObject spawnedObject)
    {
        spawnedObject.GetComponent<Rigidbody>().AddTorque(torque);
    }
}

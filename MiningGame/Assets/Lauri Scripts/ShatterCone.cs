using UnityEngine;
using System.Collections;

public class ShatterCone: MonoBehaviour {

    public static ShatterCone main;
    public static ShatterCone smaller;

    public bool smallerCone = false;

    public Vector3 furtherSphereOffset;
    public float firstSphereRadius;
    public float furtherSphereRadius;
    public float withinZaxis;

    // Use this for initialization
    void Start () {
        if (!smallerCone)
        {
            main = this;
        } else
        {
            smaller = this;
        }
        
	}

    public void DestroyCone(Vector3 position)
    {
        DestroyNeighbouringObjects(position, firstSphereRadius);
        DestroyNeighbouringObjects(position + furtherSphereOffset, furtherSphereRadius);
    }

    void DestroyNeighbouringObjects(Vector3 position, float withinDistance)
    {
        float z = this.transform.position.z;
        Collider[] hitColliders = Physics.OverlapSphere(position, withinDistance);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (Mathf.Abs(hitColliders[i].transform.position.z - z) < withinZaxis)
                hitColliders[i].SendMessage("Shatter", SendMessageOptions.DontRequireReceiver);
            i++;
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}

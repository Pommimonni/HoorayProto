using UnityEngine;
using System.Collections;

public class WallPiece : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Debug.Log("Clicked a wallpiece");
        Destroy(this.gameObject);
        ShatterEffect.main.Play(this.transform.position);
        DestroyNeighbouringObjects(0.4f);
    }

    void DestroyNeighbouringObjects(float withinDistance)
    {
        float z = this.transform.position.z;
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, withinDistance);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (Mathf.Abs(hitColliders[i].transform.position.z - z) < 0.2f)
                hitColliders[i].SendMessage("Shatter");
            i++;
        }
    }

    public void Shatter()
    {
        Destroy(this.gameObject);
    }
}

using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

    public Rigidbody rigidBody;
    public MeshCollider col;

	// Use this for initialization
	void Start () {
        rigidBody.isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Debug.Log("Clicked a rock");
        Destroy(this.gameObject);
        ShatterEffect.main.Play(this.transform.position);
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 3);
        int i = 0;
        while (i < hitColliders.Length)
        {
            hitColliders[i].transform.parent.parent.SendMessage("Shatter");
            i++;
        }
    }

    public void Shatter()
    {
        Debug.Log("Shattering");
        //ShatterEffect.main.transform.position = this.transform.position;
        //ShatterEffect.main.Play();
        Destroy(this.gameObject);
    }

}

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
        ShatterCone.main.DestroyCone(this.transform.position);
    }

    

    public void Shatter()
    {
        Destroy(this.gameObject);
    }
}

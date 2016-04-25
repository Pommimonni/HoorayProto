using UnityEngine;
using System.Collections;

public class WallHitManager : MonoBehaviour {

    public static WallHitManager main;

    public Transform gemRevealLocation;


    void Awake()
    {
        main = this;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

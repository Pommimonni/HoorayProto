using UnityEngine;
using System.Collections;

public class ShinyEffect : MonoBehaviour {

    public float minEmission;
    public float maxEmission;
    public GameObject shinePrefab;
    ParticleSystem shiny;

    public static ShinyEffect main;

    void Awake()
    {
        shiny = GetComponent<ParticleSystem>();
        main = this;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

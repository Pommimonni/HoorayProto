using UnityEngine;
using System.Collections;

public class ShatterEffect : MonoBehaviour {

    public static ShatterEffect main;
    public ParticleSystem particleFX;
    public FMODUnity.StudioEventEmitter soundFXFmod;

	// Use this for initialization
	void Start () {
        main = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play(Vector3 location)
    {
        this.transform.position = location;
        particleFX.Play();
        if (soundFXFmod)
        {
            soundFXFmod.Play();
        }
    }
}

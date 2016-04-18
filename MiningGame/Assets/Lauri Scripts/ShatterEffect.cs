using UnityEngine;
using System.Collections.Generic;

public class ShatterEffect : MonoBehaviour {

    public static ShatterEffect main;
    public List<ParticleSystem> shatterFX;
    public FMODUnity.StudioEventEmitter soundFXFmod;

    int fxAmount = 0;
    int fxIndex = 0;
	// Use this for initialization
	void Start () {
        main = this;
        fxAmount = shatterFX.Count;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play(Vector3 location)
    {
        shatterFX[fxIndex].transform.position = location;
        shatterFX[fxIndex++].Play();
        if (fxIndex >= fxAmount) fxIndex = 0;


        if (soundFXFmod)
        {
            soundFXFmod.Play();
        }
    }
}

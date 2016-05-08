using UnityEngine;
using System.Collections.Generic;

public class ShatterEffect : MonoBehaviour {

    public static ShatterEffect main;
    public List<ParticleSystem> shatterFX;
    public FMODUnity.StudioEventEmitter soundFXFmod;

    public GameObject sparklingGemPrefab;

    int fxAmount = 0;
    int fxIndex = 0;

    public float pushSparklingGemForce = 5f;
	// Use this for initialization
	void Start () {
        main = this;
        fxAmount = shatterFX.Count;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public float extraKickForDeepGems = 3f;
    public void Play(Vector3 location, bool gemFound, PlayerInformation playerInfo)
    {
        shatterFX[fxIndex].transform.position = location;
        shatterFX[fxIndex++].Play();
        if (fxIndex >= fxAmount) fxIndex = 0;
        
        if (soundFXFmod)
        {
            soundFXFmod.Play();
        }
        if (gemFound)
        {
            Vector3 spawnLoc = location;
            Vector3 cameraPos = Camera.main.transform.position;
            Vector3 locToCam = cameraPos - spawnLoc;
            Vector3 offsetSpawn = locToCam.normalized * spawnLoc.z;
            spawnLoc += offsetSpawn;
            GameObject sparklingGem = (GameObject)GameObject.Instantiate(sparklingGemPrefab, spawnLoc, Quaternion.identity);
            sparklingGem.GetComponent<Rigidbody>().AddForce(Vector3.back * (pushSparklingGemForce + sparklingGem.transform.position.z * extraKickForDeepGems) );
            sparklingGem.GetComponent<Rigidbody>().AddTorque(Vector3.right * Random.Range(100, 200));
            sparklingGem.GetComponent<Rigidbody>().AddTorque(Vector3.up * Random.Range(100, 200));
            sparklingGem.GetComponent<CoveredGem>().myPlayer = playerInfo;
            sparklingGem.GetComponent<CoveredGem>().targetLocation = playerInfo.gemRevealLocation;
        }
        else
        {
            Common.gameMaster.EmptyHitOver();
        }
    }
}

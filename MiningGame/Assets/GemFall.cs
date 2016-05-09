using UnityEngine;
using System.Collections;

public class GemFall : MonoBehaviour {

    Rigidbody rbody;
    float gemFallPushBackForce = 100;
    bool droppingDown = false;
    
	// Update is called once per frame
	void Update () {
        if (droppingDown && IsBelowScreen())
        {
            StopAndStayPut();
        }
    }

    public void FallBelowScreenAndWait(Vector3 pos, PlayerInformation playerInfo)
    {
        rbody = GetComponent<Rigidbody>();
        Vector3 spawnLoc = pos;
        Vector3 cameraPos = playerInfo.gameCamera.transform.position;
        Vector3 locToCam = cameraPos - spawnLoc;
        Vector3 offsetSpawn = locToCam.normalized * spawnLoc.z;
        spawnLoc += offsetSpawn;
        droppingDown = true;
        rbody.AddForce(Vector3.back * gemFallPushBackForce);
        rbody.AddTorque(Vector3.right * Random.Range(100, 200));
        rbody.AddTorque(Vector3.up * Random.Range(100, 200));
        rbody.useGravity = true;
        /*
        sparklingGem.GetComponent<Rigidbody>().AddTorque(Vector3.right * Random.Range(100, 200));
        sparklingGem.GetComponent<Rigidbody>().AddTorque(Vector3.up * Random.Range(100, 200));
        sparklingGem.GetComponent<CoveredGem>().myPlayer = playerInfo;
        sparklingGem.GetComponent<CoveredGem>().targetLocation = playerInfo.gemRevealLocation;
        */
    }

    void StopAndStayPut()
    {
        rbody.velocity = Vector3.zero;
        rbody.angularVelocity = Vector3.zero;
        rbody.rotation = Quaternion.identity;
        rbody.useGravity = false;
        droppingDown = false;
    }

    bool IsBelowScreen()
    {
        return transform.position.y < -20f;
    }
}
